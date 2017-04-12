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
using Quest.MasterPricing.Services.Data.Bulk;
using Quest.MasterPricing.Services.Data.Filters;


namespace Quest.MasterPricing.Services.Business.Bulk
{
    public class BulkUpdateMgr : Mgr
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbBulkUpdateMgr _dbBulkUpdateMgr = null;
        private UserSession _userSession = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public BulkUpdateMgr()
            : base()
        {
            initialize();
        }
        public BulkUpdateMgr(UserSession userSession)
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
        public questStatus PerformBulkUpdate(BulkUpdateRequest bulkUpdateRequest, out int numRows)
        {
            // Initialize
            questStatus status = null;
            numRows = -1;

            // Get the filter
            Filter filter = null;
            FilterId filterId = new FilterId(bulkUpdateRequest.FilterId);
            FilterMgr filterMgr = new FilterMgr(this.UserSession);
            status = filterMgr.GetFilter(filterId, out filter);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            bulkUpdateRequest.Filter = filter;



            // Determine if bulk update filter procedure exists.
            FilterProcedure filterProcedure = null;
            status = _dbBulkUpdateMgr.GetFilterProcedure(bulkUpdateRequest, "Update", out filterProcedure);
            if (!questStatusDef.IsSuccessOrWarning(status))
            {
                return (status);
            }

            //  Perform bulk update filter procedure if exists.
            if (questStatusDef.IsSuccess(status))
            {
                return (PerformBulkUpdateFilterProcedure(bulkUpdateRequest, filterProcedure));
            }




            // Generate the SQL
            status = _dbBulkUpdateMgr.GenerateBulkUpdateSQL(bulkUpdateRequest);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }


            // Perform bulk update.
            status = _dbBulkUpdateMgr.PerformBulkUpdate(bulkUpdateRequest, out numRows);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus PerformBulkUpdateFilterProcedure(BulkUpdateRequest bulkUpdateRequest, FilterProcedure filterProcedure)
        {
            // Initialize
            questStatus status = null;


            // Execute filter
            RunFilterRequest runFilterRequest = new RunFilterRequest();
            runFilterRequest.FilterId.Id = bulkUpdateRequest.FilterId;
            ResultsSet resultsSet = null;
            FilterMgr filterMgr = new FilterMgr(this.UserSession);
            status = filterMgr.ExecuteFilter(runFilterRequest, out resultsSet);
            if (!questStatusDef.IsSuccess(status))
            {
                return (new questStatus(status.Severity, String.Format("Error executing filter Id={0}: {1}",
                    runFilterRequest.FilterId.Id, status.Message)));
            }


            // Perform operation.
            status = _dbBulkUpdateMgr.PerformBulkUpdateFilterProcedure(bulkUpdateRequest, filterProcedure, resultsSet);
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
                _dbBulkUpdateMgr = new DbBulkUpdateMgr(this.UserSession);
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

