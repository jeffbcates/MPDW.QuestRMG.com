using System;
using System.Collections.Generic;
using System.Web;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.MPDW.Modelers;
using Quest.MasterPricing.DataMgr.Models;
using Quest.Functional.MasterPricing;
using Quest.MasterPricing.Services.Business.Bulk;


namespace Quest.MasterPricing.DataMgr.Modelers
{
    public class BulkUpdateModeler : DataMgrBaseModeler
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
        public BulkUpdateModeler(HttpRequestBase httpRequestBase, UserSession userSession)
            : base(httpRequestBase, userSession)
        {
            initialize();
        }
        public BulkUpdateModeler(HttpRequestBase httpRequestBase, UserSession userSession, DataMgrBaseViewModel dataMgrBaseViewModel)
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
        public questStatus DoBulkUpdate(BulkUpdateViewModel bulkUpdateViewModel, out int numRows)
        {
            // Initialize
            questStatus status = null;
            numRows = -1;


            // TODO: PREP REQUEST
            BulkUpdateRequest bulkUpdateRequest = new BulkUpdateRequest();
            bulkUpdateRequest.FilterId = bulkUpdateViewModel.FilterId;
            foreach (NameValueViewModel nameValueViewModel in bulkUpdateViewModel.ColumnData)
            {
                BulkUpdateColumnValue bulkUpdateColumnValue = new BulkUpdateColumnValue();
                bulkUpdateColumnValue.Name = nameValueViewModel.ColumnName;
                bulkUpdateColumnValue.Value = nameValueViewModel.ColumnValue;
                BufferMgr.TransferBuffer(nameValueViewModel, bulkUpdateColumnValue);
                bulkUpdateRequest.Columns.Add(bulkUpdateColumnValue);
            }


            // Perform bulk update.
            BulkUpdateMgr bulkUpdateMgr = new BulkUpdateMgr(this.UserSession);
            status = bulkUpdateMgr.PerformBulkUpdate(bulkUpdateRequest, out numRows);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (status);
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