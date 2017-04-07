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
    public class DbStoredProcedureLogsMgr : DbMgrSessionBased
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
        public DbStoredProcedureLogsMgr(UserSession userSession)
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
            questStatus status = null;
            storedProcedureLogId = null;


            // Data rules.


            // Create the storedProcedureLog
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = create(dbContext, storedProcedureLog, out storedProcedureLogId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.Logging.StoredProcedureLog storedProcedureLog, out Quest.Functional.Logging.StoredProcedureLogId storedProcedureLogId)
        {
            // Initialize
            questStatus status = null;
            storedProcedureLogId = null;


            // Data rules.


            // Create the storedProcedureLog in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, storedProcedureLog, out storedProcedureLogId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(Quest.Functional.Logging.StoredProcedureLogId storedProcedureLogId, out Quest.Functional.Logging.StoredProcedureLog storedProcedureLog)
        {
            // Initialize
            questStatus status = null;
            storedProcedureLog = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.StoredProcedureLogs _storedProcedureLog = null;
                status = read(dbContext, storedProcedureLogId, out _storedProcedureLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                storedProcedureLog = new Quest.Functional.Logging.StoredProcedureLog();
                BufferMgr.TransferBuffer(_storedProcedureLog, storedProcedureLog);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(string username, out Quest.Functional.Logging.StoredProcedureLog storedProcedureLog)
        {
            // Initialize
            questStatus status = null;
            storedProcedureLog = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.StoredProcedureLogs _storedProcedureLog = null;
                status = read(dbContext, username, out _storedProcedureLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                storedProcedureLog = new Quest.Functional.Logging.StoredProcedureLog();
                BufferMgr.TransferBuffer(_storedProcedureLog, storedProcedureLog);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, Quest.Functional.Logging.StoredProcedureLogId storedProcedureLogId, out Quest.Functional.Logging.StoredProcedureLog storedProcedureLog)
        {
            // Initialize
            questStatus status = null;
            storedProcedureLog = null;


            // Perform read.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.StoredProcedureLogs _storedProcedureLog = null;
                status = read((MasterPricingEntities)trans.DbContext, storedProcedureLogId, out _storedProcedureLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                storedProcedureLog = new Quest.Functional.Logging.StoredProcedureLog();
                BufferMgr.TransferBuffer(_storedProcedureLog, storedProcedureLog);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.Logging.StoredProcedureLog storedProcedureLog)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = update(dbContext, storedProcedureLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Quest.Functional.Logging.StoredProcedureLog storedProcedureLog)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((MasterPricingEntities)trans.DbContext, storedProcedureLog);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(Quest.Functional.Logging.StoredProcedureLogId storedProcedureLogId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, storedProcedureLogId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, Quest.Functional.Logging.StoredProcedureLogId storedProcedureLogId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, storedProcedureLogId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.Logging.StoredProcedureLog> storedProcedureLogList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            storedProcedureLogList = null;
            queryResponse = null;


            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.MasterPricing.StoredProcedureLogs).GetProperties().ToArray();
                        int totalRecords = dbContext.StoredProcedureLogs.Where(BuildWhereClause(queryOptions, dbProperties)).Count();
                        List<Quest.Services.Dbio.MasterPricing.StoredProcedureLogs> _storedProcedureLogsList = dbContext.StoredProcedureLogs.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_storedProcedureLogsList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        storedProcedureLogList = new List<Quest.Functional.Logging.StoredProcedureLog>();
                        foreach (Quest.Services.Dbio.MasterPricing.StoredProcedureLogs _storedProcedureLog in _storedProcedureLogsList)
                        {
                            Quest.Functional.Logging.StoredProcedureLog storedProcedureLog = new Quest.Functional.Logging.StoredProcedureLog();
                            BufferMgr.TransferBuffer(_storedProcedureLog, storedProcedureLog);
                            storedProcedureLogList.Add(storedProcedureLog);
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


        #region StoredProcedureLogs
        /*----------------------------------------------------------------------------------------------------------------------------------
         * StoredProcedureLogs
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(MasterPricingEntities dbContext, Quest.Functional.Logging.StoredProcedureLog storedProcedureLog, out StoredProcedureLogId storedProcedureLogId)
        {
            // Initialize
            storedProcedureLogId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.MasterPricing.StoredProcedureLogs _storedProcedureLog = new Quest.Services.Dbio.MasterPricing.StoredProcedureLogs();
                BufferMgr.TransferBuffer(storedProcedureLog, _storedProcedureLog);
                dbContext.StoredProcedureLogs.Add(_storedProcedureLog);
                dbContext.SaveChanges();
                if (_storedProcedureLog.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Quest.Functional.Logging.StoredProcedureLog not created"));
                }
                storedProcedureLogId = new StoredProcedureLogId(_storedProcedureLog.Id);
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus read(MasterPricingEntities dbContext, Quest.Functional.Logging.StoredProcedureLogId storedProcedureLogId, out Quest.Services.Dbio.MasterPricing.StoredProcedureLogs storedProcedureLog)
        {
            // Initialize
            storedProcedureLog = null;


            try
            {
                storedProcedureLog = dbContext.StoredProcedureLogs.Where(r => r.Id == storedProcedureLogId.Id).SingleOrDefault();
                if (storedProcedureLog == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", storedProcedureLogId.Id))));
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
        private questStatus read(MasterPricingEntities dbContext, string username, out Quest.Services.Dbio.MasterPricing.StoredProcedureLogs storedProcedureLog)
        {
            // Initialize
            storedProcedureLog = null;


            try
            {
                storedProcedureLog = dbContext.StoredProcedureLogs.Where(r => r.Username == username).SingleOrDefault();
                if (storedProcedureLog == null)
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
        private questStatus update(MasterPricingEntities dbContext, Quest.Functional.Logging.StoredProcedureLog storedProcedureLog)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                StoredProcedureLogId storedProcedureLogId = new StoredProcedureLogId(storedProcedureLog.Id);
                Quest.Services.Dbio.MasterPricing.StoredProcedureLogs _storedProcedureLog = null;
                status = read(dbContext, storedProcedureLogId, out _storedProcedureLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(storedProcedureLog, _storedProcedureLog);
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
        private questStatus delete(MasterPricingEntities dbContext, Quest.Functional.Logging.StoredProcedureLogId storedProcedureLogId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                Quest.Services.Dbio.MasterPricing.StoredProcedureLogs _storedProcedureLog = null;
                status = read(dbContext, storedProcedureLogId, out _storedProcedureLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.StoredProcedureLogs.Remove(_storedProcedureLog);
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
