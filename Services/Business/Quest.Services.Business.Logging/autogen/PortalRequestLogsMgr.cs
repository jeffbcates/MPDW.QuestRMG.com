using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.Functional.Logging;
using Quest.MPDW.Services.Data;
using Quest.MPDW.Services.Business;
using Quest.Services.Data.Logging;


namespace Quest.Services.Business.Logging
{
    public class PortalRequestLogsMgr : MgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbPortalRequestLogsMgr _dbPortalRequestLogsMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public PortalRequestLogsMgr(UserSession userSession)
            : base(userSession)
        {
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
        public questStatus Create(Quest.Functional.Logging.PortalRequestLog portalRequestLog, out Quest.Functional.Logging.PortalRequestLogId portalRequestLogId)
        {
            // Initialize
            portalRequestLogId = null;
            questStatus status = null;

            // Date/time stamp it
            portalRequestLog.Created = DateTime.Now;


            // Create portalRequestLog
            status = _dbPortalRequestLogsMgr.Create(portalRequestLog, out portalRequestLogId);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(Quest.Functional.Logging.PortalRequestLogId portalRequestLogId, out Quest.Functional.Logging.PortalRequestLog portalRequestLog)
        {
            // Initialize
            portalRequestLog = null;
            questStatus status = null;


            // Read portalRequestLog
            status = _dbPortalRequestLogsMgr.Read(portalRequestLogId, out portalRequestLog);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.Logging.PortalRequestLog portalRequestLog)
        {
            // Initialize
            questStatus status = null;


            // Update portalRequestLog
            status = _dbPortalRequestLogsMgr.Update(portalRequestLog);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(Quest.Functional.Logging.PortalRequestLogId portalRequestLogId)
        {
            // Initialize
            questStatus status = null;


            // Delete portalRequestLog
            status = _dbPortalRequestLogsMgr.Delete(portalRequestLogId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.Logging.PortalRequestLog> portalRequestLogList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            portalRequestLogList = null;


            // List
            status = _dbPortalRequestLogsMgr.List(queryOptions, out portalRequestLogList, out queryResponse);
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
                _dbPortalRequestLogsMgr = new DbPortalRequestLogsMgr(this.UserSession);
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

