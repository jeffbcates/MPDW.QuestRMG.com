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
    public class HTTPRequestLogsMgr : MgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbHTTPRequestLogsMgr _dbHTTPRequestLogsMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public HTTPRequestLogsMgr(UserSession userSession)
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
        public questStatus Create(Quest.Functional.Logging.HTTPRequestLog httpRequestLog, out Quest.Functional.Logging.HTTPRequestLogId httpRequestLogId)
        {
            // Initialize
            httpRequestLogId = null;
            questStatus status = null;

            // Date/time stamp it
            httpRequestLog.Created = DateTime.Now;


            // Create httpRequestLog
            status = _dbHTTPRequestLogsMgr.Create(httpRequestLog, out httpRequestLogId);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(Quest.Functional.Logging.HTTPRequestLogId httpRequestLogId, out Quest.Functional.Logging.HTTPRequestLog httpRequestLog)
        {
            // Initialize
            httpRequestLog = null;
            questStatus status = null;


            // Read httpRequestLog
            status = _dbHTTPRequestLogsMgr.Read(httpRequestLogId, out httpRequestLog);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.Logging.HTTPRequestLog httpRequestLog)
        {
            // Initialize
            questStatus status = null;


            // Update httpRequestLog
            status = _dbHTTPRequestLogsMgr.Update(httpRequestLog);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(Quest.Functional.Logging.HTTPRequestLogId httpRequestLogId)
        {
            // Initialize
            questStatus status = null;


            // Delete httpRequestLog
            status = _dbHTTPRequestLogsMgr.Delete(httpRequestLogId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.Logging.HTTPRequestLog> httpRequestLogList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            httpRequestLogList = null;


            // List
            status = _dbHTTPRequestLogsMgr.List(queryOptions, out httpRequestLogList, out queryResponse);
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
                _dbHTTPRequestLogsMgr = new DbHTTPRequestLogsMgr(this.UserSession);
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

