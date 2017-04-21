using System;
using System.Collections.Generic;
using System.Web;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.MPDW.Modelers;
using Quest.MasterPricing.DataMgr.Models;
using Quest.Functional.MasterPricing;
using Quest.MasterPricing.Services.Business.Tablesets;
using Quest.MasterPricing.Services.Business.Bulk;


namespace Quest.MasterPricing.DataMgr.Modelers
{
    public class BulkInsertModeler : DataMgrBaseModeler
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DataMgrBaseViewModel _dataMgrBaseViewModel = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
        * ConstructorsRequisitionListModeler
        *=================================================================================================================================*/
        public BulkInsertModeler(HttpRequestBase httpRequestBase, UserSession userSession)
            : base(httpRequestBase, userSession)
        {
            initialize();
        }
        public BulkInsertModeler(HttpRequestBase httpRequestBase, UserSession userSession, DataMgrBaseViewModel dataMgrBaseViewModel)
            : base(httpRequestBase, userSession)
        {
            this._dataMgrBaseViewModel = dataMgrBaseViewModel;
            initialize();
        }
        #endregion


        #region Public Methods
        /*==================================================================================================================================
         * Public Methods
         *=================================================================================================================================*/
        public questStatus DoBulkInsert(BulkInsertViewModel bulkInsertViewModel)
        {
            // Initialize
            questStatus status = null;


            // Build bulk insert request
            BulkInsertRequest bulkInsertRequest = new BulkInsertRequest();
            BufferMgr.TransferBuffer(bulkInsertViewModel, bulkInsertRequest, true);
            bulkInsertRequest.TablesetId = bulkInsertViewModel.TablesetId;

            foreach (BulkInsertRowDataViewModel RowData in bulkInsertViewModel.Rows)
            {
                BulkInsertRow bulkInsertRow = new BulkInsertRow();
                foreach (NameValueViewModel nameValueViewModel in RowData.Columns)
                {
                    BulkInsertColumnValue bulkInsertColumnValue = new BulkInsertColumnValue();
                    bulkInsertColumnValue.Name = nameValueViewModel.ColumnName;
                    if ((string.IsNullOrWhiteSpace(nameValueViewModel.ColumnValue)) && (nameValueViewModel.ColumnValue != null))
                    {
                            bulkInsertColumnValue.Value = nameValueViewModel.ColumnValue.Trim();
                    }
                    else
                    {
                        bulkInsertColumnValue.Value = nameValueViewModel.ColumnValue;
                    }
                    bulkInsertRow.Columns.Add(bulkInsertColumnValue);
                }
                bulkInsertRequest.Rows.Add(bulkInsertRow);
            }

            // Perform bulk insert.
            BulkInsertMgr bulkInsertMgr = new BulkInsertMgr(this.UserSession);
            status = bulkInsertMgr.PerformBulkInsert(bulkInsertRequest);
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
            return (new questStatus(Severity.Success));
        }
        #endregion
    }
}