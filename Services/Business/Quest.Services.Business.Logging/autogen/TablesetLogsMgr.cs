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
    public class TablesetLogsMgr : MgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbTablesetLogsMgr _dbTablesetLogsMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public TablesetLogsMgr(UserSession userSession)
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
        public questStatus Create(Quest.Functional.Logging.TablesetLog tablesetLog, out Quest.Functional.Logging.TablesetLogId tablesetLogId)
        {
            // Initialize
            tablesetLogId = null;
            questStatus status = null;

            // Date/time stamp it
            tablesetLog.Created = DateTime.Now;


            // Create tablesetLog
            status = _dbTablesetLogsMgr.Create(tablesetLog, out tablesetLogId);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(Quest.Functional.Logging.TablesetLogId tablesetLogId, out Quest.Functional.Logging.TablesetLog tablesetLog)
        {
            // Initialize
            tablesetLog = null;
            questStatus status = null;


            // Read tablesetLog
            status = _dbTablesetLogsMgr.Read(tablesetLogId, out tablesetLog);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.Logging.TablesetLog tablesetLog)
        {
            // Initialize
            questStatus status = null;


            // Update tablesetLog
            status = _dbTablesetLogsMgr.Update(tablesetLog);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(Quest.Functional.Logging.TablesetLogId tablesetLogId)
        {
            // Initialize
            questStatus status = null;


            // Delete tablesetLog
            status = _dbTablesetLogsMgr.Delete(tablesetLogId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(List<TablesetLogId> tablesetLogIdList)
        {
            // Initialize
            questStatus status = null;
            DbMgrTransaction trans = null;
            Mgr mgr = new Mgr(this.UserSession);


            try
            {
                // BEGIN TRANSACTION
                status = mgr.BeginTransaction("Delete_TablesetLogEntries_" + Guid.NewGuid().ToString(), out trans);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete tablesetLogId
                foreach (TablesetLogId tablesetLogId in tablesetLogIdList)
                {
                    status = _dbTablesetLogsMgr.Delete(trans, tablesetLogId);
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
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.Logging.TablesetLog> tablesetLogList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            tablesetLogList = null;


            // List
            status = _dbTablesetLogsMgr.List(queryOptions, out tablesetLogList, out queryResponse);
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
            status = _dbTablesetLogsMgr.Clear();
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
                _dbTablesetLogsMgr = new DbTablesetLogsMgr(this.UserSession);
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

