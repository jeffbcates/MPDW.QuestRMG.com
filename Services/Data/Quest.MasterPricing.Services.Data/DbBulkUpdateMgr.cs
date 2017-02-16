using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.Functional.MasterPricing;
using Quest.MPDW.Services.Data;
using Quest.Services.Dbio.MasterPricing;
using Quest.MasterPricing.Services.Data.Filters;
using Quest.MasterPricing.Services.Data.Database;


namespace Quest.MasterPricing.Services.Data.Bulk
{
    public class DbBulkUpdateMgr : DbSQLMgr
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private UserSession _userSession = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public DbBulkUpdateMgr()
            : base()
        {
            initialize();
        }
        public DbBulkUpdateMgr(UserSession userSession)
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
        #endregion


        #region Public Methods
        /*==================================================================================================================================
         * Public Methods
         *=================================================================================================================================*/
        public questStatus GenerateBulkUpdateSQL(BulkUpdateRequest bulkUpdateRequest)
        {
            // Initialize
            questStatus status = null;
            StringBuilder sbBulkUPDATE = new StringBuilder();

            // Validate
            if (bulkUpdateRequest.Filter.FilterTableList.Count != 1)
            {
                return (new questStatus(Severity.Error, String.Format("ERROR: currently, only single-table filters supported with BUlk Update")));
            }


            sbBulkUPDATE.AppendLine(String.Format("UPDATE [{0}].[{1}] SET ", 
                    bulkUpdateRequest.Filter.FilterTableList[0].TablesetTable.Table.Schema, bulkUpdateRequest.Filter.FilterTableList[0].TablesetTable.Table.Name));
            List<string> setClauseList = new List<string>();
            for (int idx=0; idx < bulkUpdateRequest.Columns.Count; idx += 1)
            {
                BulkUpdateColumnValue bulkUpdateColumnValue = bulkUpdateRequest.Columns[idx];
                string setClause = null;

                status = getFilterColumn(bulkUpdateColumnValue, bulkUpdateRequest.Filter);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                if (bulkUpdateColumnValue.bNull)
                {
                    if (setClauseList.Count > 0)
                    {
                        if (idx + 1 < bulkUpdateRequest.Columns.Count)
                        {
                            setClause = ", ";
                        }
                    }
                    setClause += String.Format(" [{0}] = NULL ", bulkUpdateColumnValue.Name);
                    setClauseList.Add(setClause);
                }
                else if (! string.IsNullOrEmpty(bulkUpdateColumnValue.Value))
                {
                    int _value = 0;
                    if (int.TryParse(bulkUpdateColumnValue.Value, out _value))
                    {
                        if (_value == -1 && bulkUpdateColumnValue.LookupId >= BaseId.VALID_ID)
                        {
                            continue;
                        }
                    }
                    if (setClauseList.Count > 0)
                    {
                        if (idx + 1 < bulkUpdateRequest.Columns.Count)
                        {
                            setClause = ", ";
                        }
                    }
                    setClause += String.Format(" [{0}] = {1} ", bulkUpdateColumnValue.Name, getValueIdentifier(bulkUpdateColumnValue));
                    setClauseList.Add(setClause);
                }
            }
            foreach (string setClause in setClauseList)
            {
                sbBulkUPDATE.AppendLine(setClause);
            }

            // KLUGIE: Get WHERE clause from Filter
            string whereClause = null;
            if (bulkUpdateRequest.Filter.SQL.IndexOf("WHERE") > -1)
            {
                // NOTE: NEED SUBCLAUSES PARSED OUT; THIS IS A QUICK KLUGIE FOR SINGLE TABLE ONLY.  BUT, MANY THINGS CAN COME AFTER A WHERE CLAUSE.
                //       WE'LL FIND OUT WHAT WHEN THE SQL HERE BOMBS OUT.
                whereClause = bulkUpdateRequest.Filter.SQL.Substring(bulkUpdateRequest.Filter.SQL.IndexOf("WHERE"));

                // Shamelessly strip off table identifer.  SINGLE TABLE ---HAS--- TO BE [T1]. TABLE ALIAS.  
                // I SHOULD BE ASHAMED OF THIS, BUT I'M NOT.  GET 'ER DONE.
                whereClause = whereClause.Replace("[T1].", "");
            }
            if (whereClause != null)
            {
                sbBulkUPDATE.AppendLine(whereClause);
            }

            // String it and return it.  Pray for the best.
            bulkUpdateRequest.SQL = sbBulkUPDATE.ToString();


            return (new questStatus(Severity.Success));
        }
        public questStatus PerformBulkUpdate(BulkUpdateRequest bulkUpdateRequest, out int numRows)
        {
            // Initialize
            questStatus status = null;
            numRows = -1;

            try
            {
                // Get database connection string
                TablesetId tablesetId = new TablesetId(bulkUpdateRequest.Filter.TablesetId);
                Tableset tableset = null;
                DbTablesetsMgr dbTablesetsMgr = new DbTablesetsMgr(this.UserSession);
                status = dbTablesetsMgr.Read(tablesetId, out tableset);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                DatabaseId databaseId = new DatabaseId(tableset.DatabaseId);
                Quest.Functional.MasterPricing.Database database = null;
                DbDatabasesMgr dbDatabasesMgr = new DbDatabasesMgr(this.UserSession);
                status = dbDatabasesMgr.Read(databaseId, out database);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                using (SqlConnection conn = new SqlConnection(database.ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(null, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = bulkUpdateRequest.SQL;
                        try
                        {
                            numRows = cmd.ExecuteNonQuery();
                            if (numRows < 0)
                            {
                                return (new questStatus(Severity.Error, String.Format("ERROR: Bulk update failed: {0}", numRows)));
                            }
                            if (numRows == 0)
                            {
                                return (new questStatus(Severity.Warning, String.Format("WARNING: {0} rows bulk updated", numRows)));
                            }
                        }
                        catch (System.Exception ex)
                        {
                            return (new questStatus(Severity.Error, String.Format("EXCEPTION: Bulk update failed: {0}",
                                    ex.Message)));
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
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
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus getFilterColumn(BulkUpdateColumnValue bulkUpdateColumnValue, Filter filter)
        {
            // Initialize
            string[] pp = null;
            string entityName = null;
            string columnName = null;
            FilterColumn filterColumn = null;


            if (filter.FilterTableList.Count == 1)
            {
                entityName = filter.FilterTableList[0].TablesetTable.Table.Name;
                columnName = bulkUpdateColumnValue.Name;
            }
            else
            {
                pp = bulkUpdateColumnValue.Name.Split('_');
                entityName = pp[0];
                columnName = pp[1];
                filterColumn = null;
            }




            // Get FilterColumn of given bulk update column.
            FilterTable filterTable = filter.FilterTableList.Find(delegate (FilterTable t) { return t.TablesetTable.Table.Name == entityName; });
            if (filterTable != null)
            {
                filterColumn = filter.FilterColumnList.Find(delegate (FilterColumn c) {
                    return c.TablesetColumn.Column.Name == columnName && c.TablesetColumn.Column.EntityTypeId == EntityType.Table && c.TablesetColumn.Column.EntityId == filterTable.TablesetTable.Table.Id;
                });
                if (filterColumn == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: filter column not found for bulk update column {0}",
                            bulkUpdateColumnValue.Name)));
                }
            }
            else
            {
                FilterView filterView = filter.FilterViewList.Find(delegate (FilterView v) { return v.TablesetView.View.Name == entityName; });
                if (filterView == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: filter view not found for bulk update column {0}",
                            bulkUpdateColumnValue.Name)));
                }
                filterColumn = filter.FilterColumnList.Find(delegate (FilterColumn c) {
                    return c.TablesetColumn.Column.Name == columnName && c.TablesetColumn.Column.EntityTypeId == EntityType.View && c.TablesetColumn.Column.EntityId == filterView.TablesetView.View.Id;
                });
                if (filterColumn == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: filter column not found for bulk update column {0} (view search)",
                            bulkUpdateColumnValue.Name)));
                }
            }
            bulkUpdateColumnValue.FilterColumn = filterColumn;

            return (new questStatus(Severity.Success));
        }
        private string getValueIdentifier(BulkUpdateColumnValue bulkUpdateColumnValue)
        {
            string valueIdentifier = null;
            bool bUseQuotes = false;


            // Figure out if quotes should be used.
            if (bulkUpdateColumnValue.FilterColumn.TablesetColumn.Column.DataTypeName.Contains("char"))
            {
                bUseQuotes = true;
            }
            else if (bulkUpdateColumnValue.FilterColumn.TablesetColumn.Column.DataTypeName.Contains("date"))
            {
                bUseQuotes = true;
            }
            else if (bulkUpdateColumnValue.FilterColumn.TablesetColumn.Column.DataTypeName.Contains("time"))
            {
                bUseQuotes = true;
            }

            // Build value assignment.
            if (bUseQuotes)
            {
                valueIdentifier = String.Format("'{0}'", bulkUpdateColumnValue.Value);
            }
            else
            {
                valueIdentifier = String.Format("{0}", bulkUpdateColumnValue.Value);
            }
            return (valueIdentifier);
        }
        #endregion
    }
}