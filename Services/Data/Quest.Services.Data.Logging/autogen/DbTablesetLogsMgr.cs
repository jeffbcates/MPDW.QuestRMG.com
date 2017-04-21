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
using Quest.MPDW.Services.Data;
using Quest.Services.Dbio.MasterPricing;


namespace Quest.Services.Data.Logging
{
    public class DbTablesetLogsMgr : DbMgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public DbTablesetLogsMgr(UserSession userSession)
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
            questStatus status = null;
            tablesetLogId = null;


            // Data rules.


            // Create the tablesetLog
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = create(dbContext, tablesetLog, out tablesetLogId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.Logging.TablesetLog tablesetLog, out Quest.Functional.Logging.TablesetLogId tablesetLogId)
        {
            // Initialize
            questStatus status = null;
            tablesetLogId = null;


            // Data rules.


            // Create the tablesetLog in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, tablesetLog, out tablesetLogId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(Quest.Functional.Logging.TablesetLogId tablesetLogId, out Quest.Functional.Logging.TablesetLog tablesetLog)
        {
            // Initialize
            questStatus status = null;
            tablesetLog = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.TablesetLogs _tablesetLog = null;
                status = read(dbContext, tablesetLogId, out _tablesetLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                tablesetLog = new Quest.Functional.Logging.TablesetLog();
                BufferMgr.TransferBuffer(_tablesetLog, tablesetLog);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(string username, out Quest.Functional.Logging.TablesetLog tablesetLog)
        {
            // Initialize
            questStatus status = null;
            tablesetLog = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.TablesetLogs _tablesetLog = null;
                status = read(dbContext, username, out _tablesetLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                tablesetLog = new Quest.Functional.Logging.TablesetLog();
                BufferMgr.TransferBuffer(_tablesetLog, tablesetLog);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, Quest.Functional.Logging.TablesetLogId tablesetLogId, out Quest.Functional.Logging.TablesetLog tablesetLog)
        {
            // Initialize
            questStatus status = null;
            tablesetLog = null;


            // Perform read.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.TablesetLogs _tablesetLog = null;
                status = read((MasterPricingEntities)trans.DbContext, tablesetLogId, out _tablesetLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                tablesetLog = new Quest.Functional.Logging.TablesetLog();
                BufferMgr.TransferBuffer(_tablesetLog, tablesetLog);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.Logging.TablesetLog tablesetLog)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = update(dbContext, tablesetLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Quest.Functional.Logging.TablesetLog tablesetLog)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((MasterPricingEntities)trans.DbContext, tablesetLog);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(Quest.Functional.Logging.TablesetLogId tablesetLogId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, tablesetLogId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, Quest.Functional.Logging.TablesetLogId tablesetLogId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, tablesetLogId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.Logging.TablesetLog> tablesetLogList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            tablesetLogList = null;
            queryResponse = null;


            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.MasterPricing.TablesetLogs).GetProperties().ToArray();
                        int totalRecords = dbContext.TablesetLogs.Where(BuildWhereClause(queryOptions, dbProperties)).Count();
                        List<Quest.Services.Dbio.MasterPricing.TablesetLogs> _tablesetLogsList = dbContext.TablesetLogs.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_tablesetLogsList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        tablesetLogList = new List<Quest.Functional.Logging.TablesetLog>();
                        foreach (Quest.Services.Dbio.MasterPricing.TablesetLogs _tablesetLog in _tablesetLogsList)
                        {
                            Quest.Functional.Logging.TablesetLog tablesetLog = new Quest.Functional.Logging.TablesetLog();
                            BufferMgr.TransferBuffer(_tablesetLog, tablesetLog);
                            tablesetLogList.Add(tablesetLog);
                        }
                        status = BuildQueryResponse(totalRecords, queryOptions, out queryResponse);
                        if (!questStatusDef.IsSuccess(status))
                        {
                            return (status);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                                this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                                ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
                    }
                }
            }
            return (new questStatus(Severity.Success));
        }

        public questStatus Clear()
        {
            try
            {
                using (MasterPricingEntities dbContext = new MasterPricingEntities())
                {
                    dbContext.TablesetLogs.RemoveRange(dbContext.TablesetLogs.Where(r => r.Id > 0));
                    dbContext.SaveChanges();
                }
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: clearing Tablesets Log {0}", ex.Message)));
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
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                throw new System.Exception(status.Message, ex);
            }
            return (new questStatus(Severity.Success));
        }


        #region TablesetLogs
        /*----------------------------------------------------------------------------------------------------------------------------------
         * TablesetLogs
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(MasterPricingEntities dbContext, Quest.Functional.Logging.TablesetLog tablesetLog, out TablesetLogId tablesetLogId)
        {
            // Initialize
            tablesetLogId = null;


            // Initialize
            tablesetLog.UserSessionId = this.UserSession.Id;
            tablesetLog.Username = this.UserSession.User.Username;
            tablesetLog.Created = DateTime.Now;


            // Perform create
            try
            {
                Quest.Services.Dbio.MasterPricing.TablesetLogs _tablesetLog = new Quest.Services.Dbio.MasterPricing.TablesetLogs();
                BufferMgr.TransferBuffer(tablesetLog, _tablesetLog);
                dbContext.TablesetLogs.Add(_tablesetLog);
                dbContext.SaveChanges();
                if (_tablesetLog.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Quest.Functional.Logging.TablesetLog not created"));
                }
                tablesetLogId = new TablesetLogId(_tablesetLog.Id);
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus read(MasterPricingEntities dbContext, Quest.Functional.Logging.TablesetLogId tablesetLogId, out Quest.Services.Dbio.MasterPricing.TablesetLogs tablesetLog)
        {
            // Initialize
            tablesetLog = null;


            try
            {
                tablesetLog = dbContext.TablesetLogs.Where(r => r.Id == tablesetLogId.Id).SingleOrDefault();
                if (tablesetLog == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", tablesetLogId.Id))));
                }
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus read(MasterPricingEntities dbContext, string username, out Quest.Services.Dbio.MasterPricing.TablesetLogs tablesetLog)
        {
            // Initialize
            tablesetLog = null;


            try
            {
                tablesetLog = dbContext.TablesetLogs.Where(r => r.Username == username).SingleOrDefault();
                if (tablesetLog == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Username {0} not found", username))));
                }
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus update(MasterPricingEntities dbContext, Quest.Functional.Logging.TablesetLog tablesetLog)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                TablesetLogId tablesetLogId = new TablesetLogId(tablesetLog.Id);
                Quest.Services.Dbio.MasterPricing.TablesetLogs _tablesetLog = null;
                status = read(dbContext, tablesetLogId, out _tablesetLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(tablesetLog, _tablesetLog);
                dbContext.SaveChanges();
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus delete(MasterPricingEntities dbContext, Quest.Functional.Logging.TablesetLogId tablesetLogId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                Quest.Services.Dbio.MasterPricing.TablesetLogs _tablesetLog = null;
                status = read(dbContext, tablesetLogId, out _tablesetLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.TablesetLogs.Remove(_tablesetLog);
                dbContext.SaveChanges();
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        #endregion

        #endregion
    }
}
