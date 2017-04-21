using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading;
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
    public class LogMgr : Mgr
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbLogMgr _dbLogMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public LogMgr()
            : base()
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
        public questStatus Log(questStatus status)
        {
            int userSessionId = BaseId.INVALID_ID;
            int severity = (int) status.Severity;
            string module = status.Module + '.' + status.Method;
            string message = status.Message;

            return log(userSessionId, severity, module, message);
        }
        public questStatus Log(UserSession userSession, questStatus status)
        {
            int userSessionId = userSession == null ? BaseId.INVALID_ID : userSession.Id;
            int severity = (int) status.Severity;
            string module = status.Module + '.' + status.Method;
            string message = status.Message;

            return log(userSessionId, severity, module, message);
        }

        #endregion


        #region CRUD
        /*----------------------------------------------------------------------------------------------------------------------------------
         * CRUD
         *---------------------------------------------------------------------------------------------------------------------------------*/
        public questStatus Read(LogId logId, out Log log)
        {
            // Initialize
            questStatus status = null;
            log = null;

            // Write to the log
            status = _dbLogMgr.Read(logId, out log);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Log> logList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            logList = null;
            queryResponse = null;


            // Read requisitionSkill
            status = _dbLogMgr.List(queryOptions, out logList, out queryResponse);
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
                _dbLogMgr = new DbLogMgr();
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                throw new System.Exception(status.Message, ex);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus log(int userSessionId, int severity, string module, string message)
        {
            // Initialize
            questStatus status = null;


            // Write to the log
            status = _dbLogMgr.Log(userSessionId, severity, module, message);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }

        #endregion
    }
}

