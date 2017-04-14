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
using Quest.Functional.FMS;
using Quest.Functional.Logging;
using Quest.Functional.MasterPricing;
using Quest.MPDW.Services.Data;
using Quest.Services.Dbio.MasterPricing;


namespace Quest.Services.Data.Logging
{
    public class DbLogsMgr : DbMgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private LogSetting _logSetting = null;
        DbExceptionLogsMgr _dbExceptionLogsMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public DbLogsMgr(UserSession userSession)
            : base(userSession)
        {
            initialize();
        }
        #endregion


        #region Properties
        /*==================================================================================================================================
         * Properties
         *=================================================================================================================================*/
        public LogSetting LogSettings
        {
            get
            {
                return (this._logSetting);
            }
        }
        public bool bLoggingExceptions
        {
            get
            {
                return (this._logSetting.bLogExceptions);
            }
        }
        #endregion


        #region Public Methods
        /*==================================================================================================================================
         * Public Methods
         *=================================================================================================================================*/

        #region Set Logging Field Routines
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Set Logging Field Routines
         *---------------------------------------------------------------------------------------------------------------------------------*/
        public questStatus SetDatabase(Quest.Functional.MasterPricing.Database database, out string Database)
        {
            if (database == null)
            {
                Database = " Id: \"null\", Name: \"null\", ConnString: \"null\" ";
            }
            else
            {
                Database = String.Format(" Id: {0}, Name: {1}, ConnString: {2} ", database.Id, database.Name,
                        database.ConnectionString == null ? "(null)" : database.ConnectionString);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus SetTableset(Tableset tableset, out string Tableset)
        {
            if (tableset == null)
            {
                Tableset = " Id: \"null\", Name: \"null\" ";
            }
            else
            {
                Tableset = String.Format("Id: {0}, Name: {1}", tableset.Id, tableset.Name);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus SetFilter(Filter filter, out string Filter)
        {
            if (filter == null)
            {
                Filter = " Id: \"null\", Name: \"null\" ";
            }
            else
            {
                Filter = String.Format(" Id: {0}, Name: {1} ", filter.Id, filter.Name);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus SetException(Exception exception, out string Exception)
        {
            // TODO: ensure supertype logged.
            Exception = String.Format(" Type: {0}, Message: {1} ", exception.GetType(), exception.Message);
            return (new questStatus(Severity.Success));
        }
        public questStatus SetArray(List<string> arrayMembers, out string array)
        {
            StringBuilder sbArray = new StringBuilder();
            sbArray.Append("[");
            for (int idx = 0; idx < arrayMembers.Count; idx++)
            {
                string jsonString = arrayMembers[idx];
                sbArray.Append("{" + jsonString + "}");
                if (idx + 1 < arrayMembers.Count)
                {
                    sbArray.Append(", ");
                }
            }
            sbArray.Append("]");
            array = sbArray.ToString();
            return (new questStatus(Severity.Success));
        }
        #endregion


        #region Logging 
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Logging
         *---------------------------------------------------------------------------------------------------------------------------------*/
        public questStatus LogException(Exception exception)
        {
            if (this.LogSettings.bLogExceptions)
            {
                return (LogException(exception, null));
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus LogException(Exception exception, questStatus Status)
        {
            // Initialize
            questStatus status = null;


            // If NOT logging, return.
            if (! this.LogSettings.bLogExceptions)
            {
                return (new questStatus(Severity.Success));
            }


            // Put exception into log entry format. 
            ExceptionLog exceptionLog = new ExceptionLog();
            questStatus moduleStatus = null;
            status = GetCaller(this.GetType(), out moduleStatus);
            if (!this.LogSettings.bLogExceptions)
            {
                return (new questStatus(Severity.Success));
            }

            exceptionLog.Module = moduleStatus.ToString();
            exceptionLog.Message = exception.Message;
            exceptionLog.Status = Status == null ? null : Status.ToString();
            exceptionLog.StackTrace = GetFullStackTrace(this.GetType(), "System.Web.Mvc");


            // Create exception log entry
            status = LogException(exceptionLog);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus LogException(ExceptionLog exceptionLog)
        {
            // Initialize
            questStatus status = null;


            // If NOT logging, return.
            if (!this.LogSettings.bLogExceptions)
            {
                return (new questStatus(Severity.Success));
            }

            // Create exception log entry
            ExceptionLogId exceptionLogId = null;
            status = _dbExceptionLogsMgr.Create(exceptionLog, out exceptionLogId);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        #endregion

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
                initLogging();
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                throw new System.Exception(status.Message, ex);
            }
            return (new questStatus(Severity.Success));
        }


        #region Logging
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Logging
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus initLogging()
        {
            // Initialize
            questStatus status = null;

            status = loadLogSettings();
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            status = initLogger();
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus loadLogSettings()
        {
            // Initialize
            questStatus status = null;


            // Get log settings.
            LogSetting logSetting = null;
            DbLogSettingsMgr dbLogSettingsMgr = new DbLogSettingsMgr(this.UserSession);
            status = dbLogSettingsMgr.Read(out logSetting);
            if (!questStatusDef.IsSuccess(status))
            {
                this._logSetting = new LogSetting();
                return (status);
            }
            this._logSetting = logSetting;


            return (new questStatus(Severity.Success));
        }
        private questStatus initLogger()
        {
            // Initialize
            questStatus status = null;


            if (this.LogSettings.bLogExceptions)
            {
                try
                {
                    _dbExceptionLogsMgr = new DbExceptionLogsMgr(this.UserSession);
                }
                catch (System.Exception ex)
                {
                    status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                            this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        #endregion

        #endregion
    }
}
