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
    public class DatabaseLogsMgr : MgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbDatabaseLogsMgr _dbDatabaseLogsMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public DatabaseLogsMgr(UserSession userSession)
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
        public questStatus Create(Quest.Functional.Logging.DatabaseLog databaseLog, out Quest.Functional.Logging.DatabaseLogId databaseLogId)
        {
            // Initialize
            databaseLogId = null;
            questStatus status = null;

            // Date/time stamp it
            databaseLog.Created = DateTime.Now;


            // Create databaseLog
            status = _dbDatabaseLogsMgr.Create(databaseLog, out databaseLogId);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(Quest.Functional.Logging.DatabaseLogId databaseLogId, out Quest.Functional.Logging.DatabaseLog databaseLog)
        {
            // Initialize
            databaseLog = null;
            questStatus status = null;


            // Read databaseLog
            status = _dbDatabaseLogsMgr.Read(databaseLogId, out databaseLog);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.Logging.DatabaseLog databaseLog)
        {
            // Initialize
            questStatus status = null;


            // Update databaseLog
            status = _dbDatabaseLogsMgr.Update(databaseLog);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(Quest.Functional.Logging.DatabaseLogId databaseLogId)
        {
            // Initialize
            questStatus status = null;


            // Delete databaseLog
            status = _dbDatabaseLogsMgr.Delete(databaseLogId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(List<DatabaseLogId> databaseLogIdList)
        {
            // Initialize
            questStatus status = null;
            DbMgrTransaction trans = null;
            Mgr mgr = new Mgr(this.UserSession);


            try
            {
                // BEGIN TRANSACTION
                status = mgr.BeginTransaction("Delete_DatabaseLogEntries_" + Guid.NewGuid().ToString(), out trans);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete databaseLogId
                foreach (DatabaseLogId databaseLogId in databaseLogIdList)
                {
                    status = _dbDatabaseLogsMgr.Delete(trans, databaseLogId);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        mgr.RollbackTransaction(trans);
                        return (status);
                    }
                }

                // COMMIT TRANSACTION
                status = mgr.CommitTransaction(trans);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            catch (System.Exception ex)
            {
                if (trans != null)
                {
                    mgr.RollbackTransaction(trans);
                }
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.Logging.DatabaseLog> databaseLogList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            databaseLogList = null;


            // List
            status = _dbDatabaseLogsMgr.List(queryOptions, out databaseLogList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }

        public questStatus Clear()
        {
            // Initialize
            questStatus status = null;


            // Clear
            status = _dbDatabaseLogsMgr.Clear();
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
                _dbDatabaseLogsMgr = new DbDatabaseLogsMgr(this.UserSession);
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

