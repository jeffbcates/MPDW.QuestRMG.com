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
using Quest.Functional.MasterPricing;
using Quest.Functional.Logging;
using Quest.Services.Data.Logging;
using Quest.MPDW.Services.Data;
using Quest.Services.Dbio.MasterPricing;
using Quest.MasterPricing.Services.Data.Database;


namespace Quest.MasterPricing.Services.Data.Filters
{
    public class DbSQLMgr : DbMgr
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private UserSession _userSession = null;
        private LogSetting _logSetting = null;
        DbDatabaseLogsMgr _dbDatabaseLogsMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public DbSQLMgr()
            : base()
        {
            initialize();
        }
        public DbSQLMgr(UserSession userSession)
            : base()
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

        #region Logging
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Logging
         *---------------------------------------------------------------------------------------------------------------------------------*/
        public questStatus LogDatabaseEvent(DatabaseLog databaseLog)
        {
            // Initialize
            questStatus status = null;

            
            // Check if logging on.
            if (! this._logSetting.bLogDatabases)
            {
                return (new questStatus(Severity.Warning, "Database Logging OFF"));
            }


            // Log event
            DatabaseLogId databaseLogId = null;
            status = _dbDatabaseLogsMgr.Create(databaseLog, out databaseLogId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            databaseLog.Id = databaseLogId.Id;


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
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
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


            if (this._logSetting.bLogDatabases)
            {
                try
                {
                    _dbDatabaseLogsMgr = new DbDatabaseLogsMgr(this.UserSession);
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
    }
}