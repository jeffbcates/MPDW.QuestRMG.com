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
    public class BulkUpdateLogsMgr : MgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbBulkUpdateLogsMgr _dbBulkUpdateLogsMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public BulkUpdateLogsMgr(UserSession userSession)
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
        public questStatus Create(Quest.Functional.Logging.BulkUpdateLog bulkUpdateLog, out Quest.Functional.Logging.BulkUpdateLogId bulkUpdateLogId)
        {
            // Initialize
            bulkUpdateLogId = null;
            questStatus status = null;

            // Date/time stamp it
            bulkUpdateLog.Created = DateTime.Now;


            // Create bulkUpdateLog
            status = _dbBulkUpdateLogsMgr.Create(bulkUpdateLog, out bulkUpdateLogId);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(Quest.Functional.Logging.BulkUpdateLogId bulkUpdateLogId, out Quest.Functional.Logging.BulkUpdateLog bulkUpdateLog)
        {
            // Initialize
            bulkUpdateLog = null;
            questStatus status = null;


            // Read bulkUpdateLog
            status = _dbBulkUpdateLogsMgr.Read(bulkUpdateLogId, out bulkUpdateLog);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.Logging.BulkUpdateLog bulkUpdateLog)
        {
            // Initialize
            questStatus status = null;


            // Update bulkUpdateLog
            status = _dbBulkUpdateLogsMgr.Update(bulkUpdateLog);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(Quest.Functional.Logging.BulkUpdateLogId bulkUpdateLogId)
        {
            // Initialize
            questStatus status = null;


            // Delete bulkUpdateLog
            status = _dbBulkUpdateLogsMgr.Delete(bulkUpdateLogId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(List<BulkUpdateLogId> bulkUpdateLogIdList)
        {
            // Initialize
            questStatus status = null;
            DbMgrTransaction trans = null;
            Mgr mgr = new Mgr(this.UserSession);


            try
            {
                // BEGIN TRANSACTION
                status = mgr.BeginTransaction("Delete_BulkUpdateLogEntries_" + Guid.NewGuid().ToString(), out trans);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete bulkUpdateLogId
                foreach (BulkUpdateLogId bulkUpdateLogId in bulkUpdateLogIdList)
                {
                    status = _dbBulkUpdateLogsMgr.Delete(trans, bulkUpdateLogId);
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
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.Logging.BulkUpdateLog> bulkUpdateLogList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            bulkUpdateLogList = null;


            // List
            status = _dbBulkUpdateLogsMgr.List(queryOptions, out bulkUpdateLogList, out queryResponse);
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
            status = _dbBulkUpdateLogsMgr.Clear();
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
                _dbBulkUpdateLogsMgr = new DbBulkUpdateLogsMgr(this.UserSession);
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

