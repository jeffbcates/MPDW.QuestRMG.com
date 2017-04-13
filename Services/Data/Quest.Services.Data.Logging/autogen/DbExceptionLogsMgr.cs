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
    public class DbExceptionLogsMgr : DbMgrSessionBased
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
        public DbExceptionLogsMgr(UserSession userSession)
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
            questStatus status = null;
            exceptionLogId = null;


            // Data rules.


            // Create the exceptionLog
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = create(dbContext, exceptionLog, out exceptionLogId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.Logging.ExceptionLog exceptionLog, out Quest.Functional.Logging.ExceptionLogId exceptionLogId)
        {
            // Initialize
            questStatus status = null;
            exceptionLogId = null;


            // Data rules.


            // Create the exceptionLog in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, exceptionLog, out exceptionLogId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(Quest.Functional.Logging.ExceptionLogId exceptionLogId, out Quest.Functional.Logging.ExceptionLog exceptionLog)
        {
            // Initialize
            questStatus status = null;
            exceptionLog = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.ExceptionLogs _exceptionLog = null;
                status = read(dbContext, exceptionLogId, out _exceptionLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                exceptionLog = new Quest.Functional.Logging.ExceptionLog();
                BufferMgr.TransferBuffer(_exceptionLog, exceptionLog);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(string username, out Quest.Functional.Logging.ExceptionLog exceptionLog)
        {
            // Initialize
            questStatus status = null;
            exceptionLog = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.ExceptionLogs _exceptionLog = null;
                status = read(dbContext, username, out _exceptionLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                exceptionLog = new Quest.Functional.Logging.ExceptionLog();
                BufferMgr.TransferBuffer(_exceptionLog, exceptionLog);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, Quest.Functional.Logging.ExceptionLogId exceptionLogId, out Quest.Functional.Logging.ExceptionLog exceptionLog)
        {
            // Initialize
            questStatus status = null;
            exceptionLog = null;


            // Perform read.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.ExceptionLogs _exceptionLog = null;
                status = read((MasterPricingEntities)trans.DbContext, exceptionLogId, out _exceptionLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                exceptionLog = new Quest.Functional.Logging.ExceptionLog();
                BufferMgr.TransferBuffer(_exceptionLog, exceptionLog);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.Logging.ExceptionLog exceptionLog)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = update(dbContext, exceptionLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Quest.Functional.Logging.ExceptionLog exceptionLog)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((MasterPricingEntities)trans.DbContext, exceptionLog);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(Quest.Functional.Logging.ExceptionLogId exceptionLogId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, exceptionLogId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, Quest.Functional.Logging.ExceptionLogId exceptionLogId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, exceptionLogId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.Logging.ExceptionLog> exceptionLogList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            exceptionLogList = null;
            queryResponse = null;


            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.MasterPricing.ExceptionLogs).GetProperties().ToArray();
                        int totalRecords = dbContext.ExceptionLogs.Where(BuildWhereClause(queryOptions, dbProperties)).Count();
                        List<Quest.Services.Dbio.MasterPricing.ExceptionLogs> _exceptionLogsList = dbContext.ExceptionLogs.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_exceptionLogsList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        exceptionLogList = new List<Quest.Functional.Logging.ExceptionLog>();
                        foreach (Quest.Services.Dbio.MasterPricing.ExceptionLogs _exceptionLog in _exceptionLogsList)
                        {
                            Quest.Functional.Logging.ExceptionLog exceptionLog = new Quest.Functional.Logging.ExceptionLog();
                            BufferMgr.TransferBuffer(_exceptionLog, exceptionLog);
                            exceptionLogList.Add(exceptionLog);
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
                    dbContext.ExceptionLogs.RemoveRange(dbContext.ExceptionLogs.Where(r => r.Id > 0));
                    dbContext.SaveChanges();
                }
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: clearing Exceptions Log {0}", ex.Message)));
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


        #region ExceptionLogs
        /*----------------------------------------------------------------------------------------------------------------------------------
         * ExceptionLogs
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(MasterPricingEntities dbContext, Quest.Functional.Logging.ExceptionLog exceptionLog, out ExceptionLogId exceptionLogId)
        {
            // Initialize
            exceptionLogId = null;


            // Initialize
            exceptionLog.UserSessionId = this.UserSession.Id;
            exceptionLog.Username = this.UserSession.User.Username;
            exceptionLog.Created = DateTime.Now;


            // Perform create
            try
            {
                Quest.Services.Dbio.MasterPricing.ExceptionLogs _exceptionLog = new Quest.Services.Dbio.MasterPricing.ExceptionLogs();
                BufferMgr.TransferBuffer(exceptionLog, _exceptionLog);
                dbContext.ExceptionLogs.Add(_exceptionLog);
                dbContext.SaveChanges();
                if (_exceptionLog.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Quest.Functional.Logging.ExceptionLog not created"));
                }
                exceptionLogId = new ExceptionLogId(_exceptionLog.Id);
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus read(MasterPricingEntities dbContext, Quest.Functional.Logging.ExceptionLogId exceptionLogId, out Quest.Services.Dbio.MasterPricing.ExceptionLogs exceptionLog)
        {
            // Initialize
            exceptionLog = null;


            try
            {
                exceptionLog = dbContext.ExceptionLogs.Where(r => r.Id == exceptionLogId.Id).SingleOrDefault();
                if (exceptionLog == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", exceptionLogId.Id))));
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
        private questStatus read(MasterPricingEntities dbContext, string username, out Quest.Services.Dbio.MasterPricing.ExceptionLogs exceptionLog)
        {
            // Initialize
            exceptionLog = null;


            try
            {
                exceptionLog = dbContext.ExceptionLogs.Where(r => r.Username == username).SingleOrDefault();
                if (exceptionLog == null)
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
        private questStatus update(MasterPricingEntities dbContext, Quest.Functional.Logging.ExceptionLog exceptionLog)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                ExceptionLogId exceptionLogId = new ExceptionLogId(exceptionLog.Id);
                Quest.Services.Dbio.MasterPricing.ExceptionLogs _exceptionLog = null;
                status = read(dbContext, exceptionLogId, out _exceptionLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(exceptionLog, _exceptionLog);
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
        private questStatus delete(MasterPricingEntities dbContext, Quest.Functional.Logging.ExceptionLogId exceptionLogId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                Quest.Services.Dbio.MasterPricing.ExceptionLogs _exceptionLog = null;
                status = read(dbContext, exceptionLogId, out _exceptionLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.ExceptionLogs.Remove(_exceptionLog);
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
