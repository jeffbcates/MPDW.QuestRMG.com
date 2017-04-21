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
    public class BulkInsertLogsMgr : MgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbBulkInsertLogsMgr _dbBulkInsertLogsMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public BulkInsertLogsMgr(UserSession userSession)
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
        public questStatus Create(Quest.Functional.Logging.BulkInsertLog bulkInsertLog, out Quest.Functional.Logging.BulkInsertLogId bulkInsertLogId)
        {
            // Initialize
            bulkInsertLogId = null;
            questStatus status = null;

            // Date/time stamp it
            bulkInsertLog.Created = DateTime.Now;


            // Create bulkInsertLog
            status = _dbBulkInsertLogsMgr.Create(bulkInsertLog, out bulkInsertLogId);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(Quest.Functional.Logging.BulkInsertLogId bulkInsertLogId, out Quest.Functional.Logging.BulkInsertLog bulkInsertLog)
        {
            // Initialize
            bulkInsertLog = null;
            questStatus status = null;


            // Read bulkInsertLog
            status = _dbBulkInsertLogsMgr.Read(bulkInsertLogId, out bulkInsertLog);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.Logging.BulkInsertLog bulkInsertLog)
        {
            // Initialize
            questStatus status = null;


            // Update bulkInsertLog
            status = _dbBulkInsertLogsMgr.Update(bulkInsertLog);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(Quest.Functional.Logging.BulkInsertLogId bulkInsertLogId)
        {
            // Initialize
            questStatus status = null;


            // Delete bulkInsertLog
            status = _dbBulkInsertLogsMgr.Delete(bulkInsertLogId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(List<BulkInsertLogId> bulkInsertLogIdList)
        {
            // Initialize
            questStatus status = null;
            DbMgrTransaction trans = null;
            Mgr mgr = new Mgr(this.UserSession);


            try
            {
                // BEGIN TRANSACTION
                status = mgr.BeginTransaction("Delete_BulkInsertLogEntries_" + Guid.NewGuid().ToString(), out trans);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete bulkInsertLog
                foreach (BulkInsertLogId bulkInsertLogId in bulkInsertLogIdList)
                {
                    status = _dbBulkInsertLogsMgr.Delete(trans, bulkInsertLogId);
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
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.Logging.BulkInsertLog> bulkInsertLogList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            bulkInsertLogList = null;


            // List
            status = _dbBulkInsertLogsMgr.List(queryOptions, out bulkInsertLogList, out queryResponse);
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
            status = _dbBulkInsertLogsMgr.Clear();
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
                _dbBulkInsertLogsMgr = new DbBulkInsertLogsMgr(this.UserSession);
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

