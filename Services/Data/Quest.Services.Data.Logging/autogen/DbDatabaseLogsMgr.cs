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
    public class DbDatabaseLogsMgr : DbMgrSessionBased
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
        public DbDatabaseLogsMgr(UserSession userSession)
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
            questStatus status = null;
            databaseLogId = null;


            // Data rules.


            // Create the databaseLog
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = create(dbContext, databaseLog, out databaseLogId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.Logging.DatabaseLog databaseLog, out Quest.Functional.Logging.DatabaseLogId databaseLogId)
        {
            // Initialize
            questStatus status = null;
            databaseLogId = null;


            // Data rules.


            // Create the databaseLog in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, databaseLog, out databaseLogId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(Quest.Functional.Logging.DatabaseLogId databaseLogId, out Quest.Functional.Logging.DatabaseLog databaseLog)
        {
            // Initialize
            questStatus status = null;
            databaseLog = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.DatabaseLogs _databaseLog = null;
                status = read(dbContext, databaseLogId, out _databaseLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                databaseLog = new Quest.Functional.Logging.DatabaseLog();
                BufferMgr.TransferBuffer(_databaseLog, databaseLog);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(string username, out Quest.Functional.Logging.DatabaseLog databaseLog)
        {
            // Initialize
            questStatus status = null;
            databaseLog = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.DatabaseLogs _databaseLog = null;
                status = read(dbContext, username, out _databaseLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                databaseLog = new Quest.Functional.Logging.DatabaseLog();
                BufferMgr.TransferBuffer(_databaseLog, databaseLog);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, Quest.Functional.Logging.DatabaseLogId databaseLogId, out Quest.Functional.Logging.DatabaseLog databaseLog)
        {
            // Initialize
            questStatus status = null;
            databaseLog = null;


            // Perform read.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.DatabaseLogs _databaseLog = null;
                status = read((MasterPricingEntities)trans.DbContext, databaseLogId, out _databaseLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                databaseLog = new Quest.Functional.Logging.DatabaseLog();
                BufferMgr.TransferBuffer(_databaseLog, databaseLog);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.Logging.DatabaseLog databaseLog)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = update(dbContext, databaseLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Quest.Functional.Logging.DatabaseLog databaseLog)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((MasterPricingEntities)trans.DbContext, databaseLog);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(Quest.Functional.Logging.DatabaseLogId databaseLogId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, databaseLogId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, Quest.Functional.Logging.DatabaseLogId databaseLogId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, databaseLogId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.Logging.DatabaseLog> databaseLogList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            databaseLogList = null;
            queryResponse = null;


            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.MasterPricing.DatabaseLogs).GetProperties().ToArray();
                        int totalRecords = dbContext.DatabaseLogs.Where(BuildWhereClause(queryOptions, dbProperties)).Count();
                        List<Quest.Services.Dbio.MasterPricing.DatabaseLogs> _databaseLogsList = dbContext.DatabaseLogs.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_databaseLogsList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        databaseLogList = new List<Quest.Functional.Logging.DatabaseLog>();
                        foreach (Quest.Services.Dbio.MasterPricing.DatabaseLogs _databaseLog in _databaseLogsList)
                        {
                            Quest.Functional.Logging.DatabaseLog databaseLog = new Quest.Functional.Logging.DatabaseLog();
                            BufferMgr.TransferBuffer(_databaseLog, databaseLog);
                            databaseLogList.Add(databaseLog);
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
                    dbContext.DatabaseLogs.RemoveRange(dbContext.DatabaseLogs.Where(r => r.Id > 0));
                    dbContext.SaveChanges();
                }
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: clearing Databases Log {0}", ex.Message)));
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


        #region DatabaseLogs
        /*----------------------------------------------------------------------------------------------------------------------------------
         * DatabaseLogs
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(MasterPricingEntities dbContext, Quest.Functional.Logging.DatabaseLog databaseLog, out DatabaseLogId databaseLogId)
        {
            // Initialize
            databaseLogId = null;


            // Initialize
            databaseLog.UserSessionId = this.UserSession.Id;
            databaseLog.Username = this.UserSession.User.Username;
            databaseLog.Created = DateTime.Now;


            // Perform create
            try
            {
                Quest.Services.Dbio.MasterPricing.DatabaseLogs _databaseLog = new Quest.Services.Dbio.MasterPricing.DatabaseLogs();
                BufferMgr.TransferBuffer(databaseLog, _databaseLog);
                dbContext.DatabaseLogs.Add(_databaseLog);
                dbContext.SaveChanges();
                if (_databaseLog.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Quest.Functional.Logging.DatabaseLog not created"));
                }
                databaseLogId = new DatabaseLogId(_databaseLog.Id);
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus read(MasterPricingEntities dbContext, Quest.Functional.Logging.DatabaseLogId databaseLogId, out Quest.Services.Dbio.MasterPricing.DatabaseLogs databaseLog)
        {
            // Initialize
            databaseLog = null;


            try
            {
                databaseLog = dbContext.DatabaseLogs.Where(r => r.Id == databaseLogId.Id).SingleOrDefault();
                if (databaseLog == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", databaseLogId.Id))));
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
        private questStatus read(MasterPricingEntities dbContext, string username, out Quest.Services.Dbio.MasterPricing.DatabaseLogs databaseLog)
        {
            // Initialize
            databaseLog = null;


            try
            {
                databaseLog = dbContext.DatabaseLogs.Where(r => r.Username == username).SingleOrDefault();
                if (databaseLog == null)
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
        private questStatus update(MasterPricingEntities dbContext, Quest.Functional.Logging.DatabaseLog databaseLog)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                DatabaseLogId databaseLogId = new DatabaseLogId(databaseLog.Id);
                Quest.Services.Dbio.MasterPricing.DatabaseLogs _databaseLog = null;
                status = read(dbContext, databaseLogId, out _databaseLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(databaseLog, _databaseLog);
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
        private questStatus delete(MasterPricingEntities dbContext, Quest.Functional.Logging.DatabaseLogId databaseLogId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                Quest.Services.Dbio.MasterPricing.DatabaseLogs _databaseLog = null;
                status = read(dbContext, databaseLogId, out _databaseLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.DatabaseLogs.Remove(_databaseLog);
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
