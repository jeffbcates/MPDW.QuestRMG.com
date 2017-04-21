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
    public class StoredProcedureLogsMgr : MgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbStoredProcedureLogsMgr _dbStoredProcedureLogsMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public StoredProcedureLogsMgr(UserSession userSession)
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
        public questStatus Create(Quest.Functional.Logging.StoredProcedureLog storedProcedureLog, out Quest.Functional.Logging.StoredProcedureLogId storedProcedureLogId)
        {
            // Initialize
            storedProcedureLogId = null;
            questStatus status = null;

            // Date/time stamp it
            storedProcedureLog.Created = DateTime.Now;


            // Create storedProcedureLog
            status = _dbStoredProcedureLogsMgr.Create(storedProcedureLog, out storedProcedureLogId);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(Quest.Functional.Logging.StoredProcedureLogId storedProcedureLogId, out Quest.Functional.Logging.StoredProcedureLog storedProcedureLog)
        {
            // Initialize
            storedProcedureLog = null;
            questStatus status = null;


            // Read storedProcedureLog
            status = _dbStoredProcedureLogsMgr.Read(storedProcedureLogId, out storedProcedureLog);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.Logging.StoredProcedureLog storedProcedureLog)
        {
            // Initialize
            questStatus status = null;


            // Update storedProcedureLog
            status = _dbStoredProcedureLogsMgr.Update(storedProcedureLog);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(Quest.Functional.Logging.StoredProcedureLogId storedProcedureLogId)
        {
            // Initialize
            questStatus status = null;


            // Delete storedProcedureLog
            status = _dbStoredProcedureLogsMgr.Delete(storedProcedureLogId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(List<StoredProcedureLogId> storedProcedureLogIdList)
        {
            // Initialize
            questStatus status = null;
            DbMgrTransaction trans = null;
            Mgr mgr = new Mgr(this.UserSession);


            try
            {
                // BEGIN TRANSACTION
                status = mgr.BeginTransaction("Delete_StoredProcedureLogEntries_" + Guid.NewGuid().ToString(), out trans);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete storedProcedureLogId
                foreach (StoredProcedureLogId storedProcedureLogId in storedProcedureLogIdList)
                {
                    status = _dbStoredProcedureLogsMgr.Delete(trans, storedProcedureLogId);
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
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.Logging.StoredProcedureLog> storedProcedureLogList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            storedProcedureLogList = null;


            // List
            status = _dbStoredProcedureLogsMgr.List(queryOptions, out storedProcedureLogList, out queryResponse);
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
            status = _dbStoredProcedureLogsMgr.Clear();
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
                _dbStoredProcedureLogsMgr = new DbStoredProcedureLogsMgr(this.UserSession);
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

