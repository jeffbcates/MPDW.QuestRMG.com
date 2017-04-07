using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.Functional.MasterPricing;
using Quest.MPDW.Services.Data;
using Quest.MPDW.Services.Business;
using Quest.MasterPricing.Services.Business.Filters;
using Quest.MasterPricing.Services.Data.Filters;
using Quest.MasterPricing.Services.Data.Bulk;


namespace Quest.MasterPricing.Services.Business.Bulk
{
    public class BulkInsertMgr : Mgr
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbBulkInsertMgr _dbBulkInsertMgr = null;
        private UserSession _userSession = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public BulkInsertMgr(UserSession userSession)
            : base(userSession)
        {
            _userSession = userSession;
            initialize();
        }
        #endregion


        #region Properties
        /*==================================================================================================================================
         * Properties
         *=================================================================================================================================*/
        public UserSession UserSession
        {
            get
            {
                return (this._userSession);
            }
        }
        #endregion


        #region Public Methods
        /*==================================================================================================================================
         * Public Methods
         *=================================================================================================================================*/
        public questStatus PerformBulkInsert(BulkInsertRequest bulkInsertRequest)
        {
            // Initialize
            questStatus status = null;


            // Read Filter
            FilterId filterId = new FilterId(bulkInsertRequest.FilterId);
            Filter filter = null;
            FiltersMgr filtersMgr = new FiltersMgr(this.UserSession);
            status = filtersMgr.Read(filterId, out filter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            bulkInsertRequest.Filter = filter;


            // Determine if bulk insert filter procedure exists.
            FilterProcedure filterProcedure = null;
            status = _dbBulkInsertMgr.GetFilterProcedure(bulkInsertRequest, "Insert", out filterProcedure);
            if (!questStatusDef.IsSuccessOrWarning(status))
            {
                return (status);
            }

            //  Perform bulk insert filter procedure if exists.
            if (questStatusDef.IsSuccess(status))
            {
                return (PerformBulkInsertFilterProcedure(bulkInsertRequest, filterProcedure));
            }

            // Validate bulk insert request
            BulkInsertResponse bulkInsertResponse = null;
            status = ValidateRequest(bulkInsertRequest, out bulkInsertResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Generate the SQL
            status = _dbBulkInsertMgr.GenerateBulkInsertSQL(bulkInsertRequest);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Perform bulk insert.
            status = _dbBulkInsertMgr.PerformBulkInsert(bulkInsertRequest);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus ValidateRequest(BulkInsertRequest bulkInsertRequest, out BulkInsertResponse bulkInsertResponse)
        {
            // Initialize
            questStatus status = null;
            bulkInsertResponse = new BulkInsertResponse();
            bulkInsertResponse.BulkInsertRequest = bulkInsertRequest;


            // Get the filter.
            FilterId filterId = new FilterId(bulkInsertRequest.FilterId);
            Filter filter = null;
            DbFilterMgr dbFilterMgr = new DbFilterMgr(this.UserSession);
            status = dbFilterMgr.GetFilter(filterId, out filter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            bulkInsertRequest.Filter = filter;


            // Validate the request.
            foreach (BulkInsertRow bulkInsertRow in bulkInsertRequest.Rows)
            {
                BulkInsertRow invalidRow = null;
                for (int rdx=0; rdx < bulkInsertRow.Columns.Count; rdx += 1)
                {
                    BulkInsertColumnValue bulkInsertColumnValue = bulkInsertRow.Columns[rdx];

                    string entityName = null;
                    string columnName = null;
                    if (filter.FilterTableList.Count == 1)
                    {
                        entityName = filter.FilterTableList[0].TablesetTable.Table.Name;
                        columnName = bulkInsertColumnValue.Name;
                    }
                    else
                    {
                        string[] pp = bulkInsertColumnValue.Name.Split('_');
                        entityName = pp[0];
                        columnName = pp[1];
                    }


                    FilterColumn filterColumn = null;
                    FilterTable filterTable = filter.FilterTableList.Find(delegate (FilterTable t) { return t.TablesetTable.Table.Name == entityName; });
                    if (filterTable != null)
                    {
                        filterColumn = filter.FilterColumnList.Find(delegate (FilterColumn c) {
                            return c.TablesetColumn.Column.Name == columnName && c.TablesetColumn.Column.EntityTypeId == EntityType.Table && c.TablesetColumn.Column.EntityId == filterTable.TablesetTable.Table.Id;
                        });
                        if (filterColumn == null)
                        {
                            return (new questStatus(Severity.Error, String.Format("ERROR: filter column not found for bulk insert column {0}, row # {1}", 
                                    bulkInsertColumnValue.Name, (rdx + 1))));
                        }
                    }
                    else
                    {
                        FilterView filterView = filter.FilterViewList.Find(delegate (FilterView v) { return v.TablesetView.View.Name == entityName; });
                        if (filterView == null)
                        {
                            return (new questStatus(Severity.Error, String.Format("ERROR: filter view not found for bulk insert column {0}, row # {1}",
                                    bulkInsertColumnValue.Name, (rdx + 1))));
                        }
                        filterColumn = filter.FilterColumnList.Find(delegate (FilterColumn c) {
                            return c.TablesetColumn.Column.Name == columnName && c.TablesetColumn.Column.EntityTypeId == EntityType.View && c.TablesetColumn.Column.EntityId == filterView.TablesetView.View.Id;
                        });
                        if (filterColumn == null)
                        {
                            return (new questStatus(Severity.Error, String.Format("ERROR: filter column not found for bulk insert column {0}, row # {1} (view search)",
                                    bulkInsertColumnValue.Name, (rdx + 1))));
                        }
                    }

                    // Validate the column
                    status = ValidateBulkInsertColumn(bulkInsertColumnValue, filterColumn);
                    if (! questStatusDef.IsSuccess(status))
                    {
                        if (invalidRow == null)
                        {
                            invalidRow = new BulkInsertRow();
                        }
                        invalidRow.Columns.Add(bulkInsertColumnValue);
                    }
                }
                if (invalidRow != null)
                {
                    bulkInsertResponse.ValidationErrors.Add(invalidRow);
                }
            }

            // TODO: IF ANY INVALID FIELDS, RETURN AN ERROR.
            if (bulkInsertResponse.ValidationErrors.Count > 0)
            {
                return (new questStatus(Severity.Error));
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus ValidateBulkInsertColumn(BulkInsertColumnValue bulkInsertColumnValue, FilterColumn filterColumn)
        {
            // Initialize
            questStatus status = null;


            // TODO: refactor
            if (filterColumn.TablesetColumn.Column.bIsIdentity)
            {
                return (new questStatus(Severity.Success));
            }
            if (filterColumn.TablesetColumn.Column.bIsAutoIncrement)
            {
                return (new questStatus(Severity.Success));
            }
            if (filterColumn.TablesetColumn.Column.bIsReadOnly)
            {
                return (new questStatus(Severity.Success));
            }
            if (! filterColumn.TablesetColumn.Column.bAllowDbNull)
            {
                if (bulkInsertColumnValue.Value == null)
                {
                    return (new questStatus(Severity.Error, String.Format("{0} value is required", bulkInsertColumnValue.Name)));
                }
                // TODO: PER COLUMN CHECKS.  FOR NOW, LET SQL ENGINE CATCH IT.
            }
            if (filterColumn.TablesetColumn.Column.LookupId.HasValue)
            {
                // TODO: Validate lookup value.
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus PerformBulkInsertFilterProcedure(BulkInsertRequest bulkInsertRequest, FilterProcedure filterProcedure)
        {
            // Initialize
            questStatus status = null;


            // Perform operation.
            status = _dbBulkInsertMgr.PerformBulkInsertFilterProcedure(bulkInsertRequest, filterProcedure);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        #endregion


        #region Private Methods
        /*==================================================================================================================================
         * Private Methods
         *=================================================================================================================================*/
        private questStatus initialize()
        {
            // Initialize
            questStatus status = null;
            try
            {
                _dbBulkInsertMgr = new DbBulkInsertMgr(this.UserSession);
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                throw new System.Exception(status.Message, ex);
            }
            return (new questStatus(Severity.Success));
        }
        #endregion
    }
}

