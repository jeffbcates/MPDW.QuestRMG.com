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
    public class DbBulkInsertLogsMgr : DbLogsMgr
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
        public DbBulkInsertLogsMgr(UserSession userSession)
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
            questStatus status = null;
            bulkInsertLogId = null;


            // Data rules.
            bulkInsertLog.Created = DateTime.Now;


            // Create the bulkInsertLog
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = create(dbContext, bulkInsertLog, out bulkInsertLogId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.Logging.BulkInsertLog bulkInsertLog, out Quest.Functional.Logging.BulkInsertLogId bulkInsertLogId)
        {
            // Initialize
            questStatus status = null;
            bulkInsertLogId = null;


            // Data rules.


            // Create the bulkInsertLog in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, bulkInsertLog, out bulkInsertLogId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(Quest.Functional.Logging.BulkInsertLogId bulkInsertLogId, out Quest.Functional.Logging.BulkInsertLog bulkInsertLog)
        {
            // Initialize
            questStatus status = null;
            bulkInsertLog = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.BulkInsertLogs _bulkInsertLog = null;
                status = read(dbContext, bulkInsertLogId, out _bulkInsertLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                bulkInsertLog = new Quest.Functional.Logging.BulkInsertLog();
                BufferMgr.TransferBuffer(_bulkInsertLog, bulkInsertLog);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(string username, out Quest.Functional.Logging.BulkInsertLog bulkInsertLog)
        {
            // Initialize
            questStatus status = null;
            bulkInsertLog = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.BulkInsertLogs _bulkInsertLog = null;
                status = read(dbContext, username, out _bulkInsertLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                bulkInsertLog = new Quest.Functional.Logging.BulkInsertLog();
                BufferMgr.TransferBuffer(_bulkInsertLog, bulkInsertLog);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, Quest.Functional.Logging.BulkInsertLogId bulkInsertLogId, out Quest.Functional.Logging.BulkInsertLog bulkInsertLog)
        {
            // Initialize
            questStatus status = null;
            bulkInsertLog = null;


            // Perform read.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.BulkInsertLogs _bulkInsertLog = null;
                status = read((MasterPricingEntities)trans.DbContext, bulkInsertLogId, out _bulkInsertLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                bulkInsertLog = new Quest.Functional.Logging.BulkInsertLog();
                BufferMgr.TransferBuffer(_bulkInsertLog, bulkInsertLog);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.Logging.BulkInsertLog bulkInsertLog)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = update(dbContext, bulkInsertLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Quest.Functional.Logging.BulkInsertLog bulkInsertLog)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((MasterPricingEntities)trans.DbContext, bulkInsertLog);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(Quest.Functional.Logging.BulkInsertLogId bulkInsertLogId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, bulkInsertLogId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, Quest.Functional.Logging.BulkInsertLogId bulkInsertLogId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, bulkInsertLogId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.Logging.BulkInsertLog> bulkInsertLogList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            bulkInsertLogList = null;
            queryResponse = null;


            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.MasterPricing.BulkInsertLogs).GetProperties().ToArray();
                        int totalRecords = dbContext.BulkInsertLogs.Where(BuildWhereClause(queryOptions, dbProperties)).Count();
                        List<Quest.Services.Dbio.MasterPricing.BulkInsertLogs> _bulkInsertLogsList = dbContext.BulkInsertLogs.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_bulkInsertLogsList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        bulkInsertLogList = new List<Quest.Functional.Logging.BulkInsertLog>();
                        foreach (Quest.Services.Dbio.MasterPricing.BulkInsertLogs _bulkInsertLog in _bulkInsertLogsList)
                        {
                            Quest.Functional.Logging.BulkInsertLog bulkInsertLog = new Quest.Functional.Logging.BulkInsertLog();
                            BufferMgr.TransferBuffer(_bulkInsertLog, bulkInsertLog);
                            bulkInsertLogList.Add(bulkInsertLog);
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


        #region Logging
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Logging
         *---------------------------------------------------------------------------------------------------------------------------------*/
        #endregion

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


        #region BulkInsertLogs
        /*----------------------------------------------------------------------------------------------------------------------------------
         * BulkInsertLogs
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(MasterPricingEntities dbContext, Quest.Functional.Logging.BulkInsertLog bulkInsertLog, out BulkInsertLogId bulkInsertLogId)
        {
            // Initialize
            bulkInsertLogId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.MasterPricing.BulkInsertLogs _bulkInsertLog = new Quest.Services.Dbio.MasterPricing.BulkInsertLogs();
                BufferMgr.TransferBuffer(bulkInsertLog, _bulkInsertLog);
                dbContext.BulkInsertLogs.Add(_bulkInsertLog);
                dbContext.SaveChanges();
                if (_bulkInsertLog.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Quest.Functional.Logging.BulkInsertLog not created"));
                }
                bulkInsertLogId = new BulkInsertLogId(_bulkInsertLog.Id);
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus read(MasterPricingEntities dbContext, Quest.Functional.Logging.BulkInsertLogId bulkInsertLogId, out Quest.Services.Dbio.MasterPricing.BulkInsertLogs bulkInsertLog)
        {
            // Initialize
            bulkInsertLog = null;


            try
            {
                bulkInsertLog = dbContext.BulkInsertLogs.Where(r => r.Id == bulkInsertLogId.Id).SingleOrDefault();
                if (bulkInsertLog == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", bulkInsertLogId.Id))));
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
        private questStatus read(MasterPricingEntities dbContext, string username, out Quest.Services.Dbio.MasterPricing.BulkInsertLogs bulkInsertLog)
        {
            // Initialize
            bulkInsertLog = null;


            try
            {
                bulkInsertLog = dbContext.BulkInsertLogs.Where(r => r.Username == username).SingleOrDefault();
                if (bulkInsertLog == null)
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
        private questStatus update(MasterPricingEntities dbContext, Quest.Functional.Logging.BulkInsertLog bulkInsertLog)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                BulkInsertLogId bulkInsertLogId = new BulkInsertLogId(bulkInsertLog.Id);
                Quest.Services.Dbio.MasterPricing.BulkInsertLogs _bulkInsertLog = null;
                status = read(dbContext, bulkInsertLogId, out _bulkInsertLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(bulkInsertLog, _bulkInsertLog);
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
        private questStatus delete(MasterPricingEntities dbContext, Quest.Functional.Logging.BulkInsertLogId bulkInsertLogId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                Quest.Services.Dbio.MasterPricing.BulkInsertLogs _bulkInsertLog = null;
                status = read(dbContext, bulkInsertLogId, out _bulkInsertLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.BulkInsertLogs.Remove(_bulkInsertLog);
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
