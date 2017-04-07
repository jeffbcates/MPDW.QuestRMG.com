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
    public class ExceptionLogsMgr : MgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbExceptionLogsMgr _dbExceptionLogsMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public ExceptionLogsMgr(UserSession userSession)
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
        public questStatus Create(Quest.Functional.Logging.ExceptionLog exceptionLog, out Quest.Functional.Logging.ExceptionLogId exceptionLogId)
        {
            // Initialize
            exceptionLogId = null;
            questStatus status = null;

            // Date/time stamp it
            exceptionLog.Created = DateTime.Now;


            // Create exceptionLog
            status = _dbExceptionLogsMgr.Create(exceptionLog, out exceptionLogId);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(Quest.Functional.Logging.ExceptionLogId exceptionLogId, out Quest.Functional.Logging.ExceptionLog exceptionLog)
        {
            // Initialize
            exceptionLog = null;
            questStatus status = null;


            // Read exceptionLog
            status = _dbExceptionLogsMgr.Read(exceptionLogId, out exceptionLog);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.Logging.ExceptionLog exceptionLog)
        {
            // Initialize
            questStatus status = null;


            // Update exceptionLog
            status = _dbExceptionLogsMgr.Update(exceptionLog);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(Quest.Functional.Logging.ExceptionLogId exceptionLogId)
        {
            // Initialize
            questStatus status = null;


            // Delete exceptionLog
            status = _dbExceptionLogsMgr.Delete(exceptionLogId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.Logging.ExceptionLog> exceptionLogList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            exceptionLogList = null;


            // List
            status = _dbExceptionLogsMgr.List(queryOptions, out exceptionLogList, out queryResponse);
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
                _dbExceptionLogsMgr = new DbExceptionLogsMgr(this.UserSession);
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

