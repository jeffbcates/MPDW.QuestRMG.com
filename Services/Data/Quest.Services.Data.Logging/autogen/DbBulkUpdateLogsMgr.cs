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
    public class DbBulkUpdateLogsMgr : DbMgrSessionBased
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
        public DbBulkUpdateLogsMgr(UserSession userSession)
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
            questStatus status = null;
            bulkUpdateLogId = null;


            // Data rules.


            // Create the bulkUpdateLog
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = create(dbContext, bulkUpdateLog, out bulkUpdateLogId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.Logging.BulkUpdateLog bulkUpdateLog, out Quest.Functional.Logging.BulkUpdateLogId bulkUpdateLogId)
        {
            // Initialize
            questStatus status = null;
            bulkUpdateLogId = null;


            // Data rules.


            // Create the bulkUpdateLog in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, bulkUpdateLog, out bulkUpdateLogId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(Quest.Functional.Logging.BulkUpdateLogId bulkUpdateLogId, out Quest.Functional.Logging.BulkUpdateLog bulkUpdateLog)
        {
            // Initialize
            questStatus status = null;
            bulkUpdateLog = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.BulkUpdateLogs _bulkUpdateLog = null;
                status = read(dbContext, bulkUpdateLogId, out _bulkUpdateLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                bulkUpdateLog = new Quest.Functional.Logging.BulkUpdateLog();
                BufferMgr.TransferBuffer(_bulkUpdateLog, bulkUpdateLog);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(string username, out Quest.Functional.Logging.BulkUpdateLog bulkUpdateLog)
        {
            // Initialize
            questStatus status = null;
            bulkUpdateLog = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.BulkUpdateLogs _bulkUpdateLog = null;
                status = read(dbContext, username, out _bulkUpdateLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                bulkUpdateLog = new Quest.Functional.Logging.BulkUpdateLog();
                BufferMgr.TransferBuffer(_bulkUpdateLog, bulkUpdateLog);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, Quest.Functional.Logging.BulkUpdateLogId bulkUpdateLogId, out Quest.Functional.Logging.BulkUpdateLog bulkUpdateLog)
        {
            // Initialize
            questStatus status = null;
            bulkUpdateLog = null;


            // Perform read.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.BulkUpdateLogs _bulkUpdateLog = null;
                status = read((MasterPricingEntities)trans.DbContext, bulkUpdateLogId, out _bulkUpdateLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                bulkUpdateLog = new Quest.Functional.Logging.BulkUpdateLog();
                BufferMgr.TransferBuffer(_bulkUpdateLog, bulkUpdateLog);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.Logging.BulkUpdateLog bulkUpdateLog)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = update(dbContext, bulkUpdateLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Quest.Functional.Logging.BulkUpdateLog bulkUpdateLog)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((MasterPricingEntities)trans.DbContext, bulkUpdateLog);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(Quest.Functional.Logging.BulkUpdateLogId bulkUpdateLogId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, bulkUpdateLogId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, Quest.Functional.Logging.BulkUpdateLogId bulkUpdateLogId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, bulkUpdateLogId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.Logging.BulkUpdateLog> bulkUpdateLogList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            bulkUpdateLogList = null;
            queryResponse = null;


            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.MasterPricing.BulkUpdateLogs).GetProperties().ToArray();
                        int totalRecords = dbContext.BulkUpdateLogs.Where(BuildWhereClause(queryOptions, dbProperties)).Count();
                        List<Quest.Services.Dbio.MasterPricing.BulkUpdateLogs> _bulkUpdateLogsList = dbContext.BulkUpdateLogs.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_bulkUpdateLogsList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        bulkUpdateLogList = new List<Quest.Functional.Logging.BulkUpdateLog>();
                        foreach (Quest.Services.Dbio.MasterPricing.BulkUpdateLogs _bulkUpdateLog in _bulkUpdateLogsList)
                        {
                            Quest.Functional.Logging.BulkUpdateLog bulkUpdateLog = new Quest.Functional.Logging.BulkUpdateLog();
                            BufferMgr.TransferBuffer(_bulkUpdateLog, bulkUpdateLog);
                            bulkUpdateLogList.Add(bulkUpdateLog);
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


        #region BulkUpdateLogs
        /*----------------------------------------------------------------------------------------------------------------------------------
         * BulkUpdateLogs
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(MasterPricingEntities dbContext, Quest.Functional.Logging.BulkUpdateLog bulkUpdateLog, out BulkUpdateLogId bulkUpdateLogId)
        {
            // Initialize
            bulkUpdateLogId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.MasterPricing.BulkUpdateLogs _bulkUpdateLog = new Quest.Services.Dbio.MasterPricing.BulkUpdateLogs();
                BufferMgr.TransferBuffer(bulkUpdateLog, _bulkUpdateLog);
                dbContext.BulkUpdateLogs.Add(_bulkUpdateLog);
                dbContext.SaveChanges();
                if (_bulkUpdateLog.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Quest.Functional.Logging.BulkUpdateLog not created"));
                }
                bulkUpdateLogId = new BulkUpdateLogId(_bulkUpdateLog.Id);
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus read(MasterPricingEntities dbContext, Quest.Functional.Logging.BulkUpdateLogId bulkUpdateLogId, out Quest.Services.Dbio.MasterPricing.BulkUpdateLogs bulkUpdateLog)
        {
            // Initialize
            bulkUpdateLog = null;


            try
            {
                bulkUpdateLog = dbContext.BulkUpdateLogs.Where(r => r.Id == bulkUpdateLogId.Id).SingleOrDefault();
                if (bulkUpdateLog == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", bulkUpdateLogId.Id))));
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
        private questStatus read(MasterPricingEntities dbContext, string username, out Quest.Services.Dbio.MasterPricing.BulkUpdateLogs bulkUpdateLog)
        {
            // Initialize
            bulkUpdateLog = null;


            try
            {
                bulkUpdateLog = dbContext.BulkUpdateLogs.Where(r => r.Username == username).SingleOrDefault();
                if (bulkUpdateLog == null)
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
        private questStatus update(MasterPricingEntities dbContext, Quest.Functional.Logging.BulkUpdateLog bulkUpdateLog)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                BulkUpdateLogId bulkUpdateLogId = new BulkUpdateLogId(bulkUpdateLog.Id);
                Quest.Services.Dbio.MasterPricing.BulkUpdateLogs _bulkUpdateLog = null;
                status = read(dbContext, bulkUpdateLogId, out _bulkUpdateLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(bulkUpdateLog, _bulkUpdateLog);
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
        private questStatus delete(MasterPricingEntities dbContext, Quest.Functional.Logging.BulkUpdateLogId bulkUpdateLogId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                Quest.Services.Dbio.MasterPricing.BulkUpdateLogs _bulkUpdateLog = null;
                status = read(dbContext, bulkUpdateLogId, out _bulkUpdateLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.BulkUpdateLogs.Remove(_bulkUpdateLog);
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
