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
    public class DbFilterLogsMgr : DbMgrSessionBased
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
        public DbFilterLogsMgr(UserSession userSession)
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
        public questStatus Create(Quest.Functional.Logging.FilterLog filterLog, out Quest.Functional.Logging.FilterLogId filterLogId)
        {
            // Initialize
            questStatus status = null;
            filterLogId = null;


            // Data rules.


            // Create the filterLog
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = create(dbContext, filterLog, out filterLogId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.Logging.FilterLog filterLog, out Quest.Functional.Logging.FilterLogId filterLogId)
        {
            // Initialize
            questStatus status = null;
            filterLogId = null;


            // Data rules.


            // Create the filterLog in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, filterLog, out filterLogId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(Quest.Functional.Logging.FilterLogId filterLogId, out Quest.Functional.Logging.FilterLog filterLog)
        {
            // Initialize
            questStatus status = null;
            filterLog = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.FilterLogs _filterLog = null;
                status = read(dbContext, filterLogId, out _filterLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterLog = new Quest.Functional.Logging.FilterLog();
                BufferMgr.TransferBuffer(_filterLog, filterLog);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(string username, out Quest.Functional.Logging.FilterLog filterLog)
        {
            // Initialize
            questStatus status = null;
            filterLog = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.FilterLogs _filterLog = null;
                status = read(dbContext, username, out _filterLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterLog = new Quest.Functional.Logging.FilterLog();
                BufferMgr.TransferBuffer(_filterLog, filterLog);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, Quest.Functional.Logging.FilterLogId filterLogId, out Quest.Functional.Logging.FilterLog filterLog)
        {
            // Initialize
            questStatus status = null;
            filterLog = null;


            // Perform read.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.FilterLogs _filterLog = null;
                status = read((MasterPricingEntities)trans.DbContext, filterLogId, out _filterLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                filterLog = new Quest.Functional.Logging.FilterLog();
                BufferMgr.TransferBuffer(_filterLog, filterLog);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.Logging.FilterLog filterLog)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = update(dbContext, filterLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Quest.Functional.Logging.FilterLog filterLog)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((MasterPricingEntities)trans.DbContext, filterLog);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(Quest.Functional.Logging.FilterLogId filterLogId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, filterLogId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, Quest.Functional.Logging.FilterLogId filterLogId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, filterLogId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.Logging.FilterLog> filterLogList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            filterLogList = null;
            queryResponse = null;


            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.MasterPricing.FilterLogs).GetProperties().ToArray();
                        int totalRecords = dbContext.FilterLogs.Where(BuildWhereClause(queryOptions, dbProperties)).Count();
                        List<Quest.Services.Dbio.MasterPricing.FilterLogs> _filterLogsList = dbContext.FilterLogs.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_filterLogsList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        filterLogList = new List<Quest.Functional.Logging.FilterLog>();
                        foreach (Quest.Services.Dbio.MasterPricing.FilterLogs _filterLog in _filterLogsList)
                        {
                            Quest.Functional.Logging.FilterLog filterLog = new Quest.Functional.Logging.FilterLog();
                            BufferMgr.TransferBuffer(_filterLog, filterLog);
                            filterLogList.Add(filterLog);
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


        #region FilterLogs
        /*----------------------------------------------------------------------------------------------------------------------------------
         * FilterLogs
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(MasterPricingEntities dbContext, Quest.Functional.Logging.FilterLog filterLog, out FilterLogId filterLogId)
        {
            // Initialize
            filterLogId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.MasterPricing.FilterLogs _filterLog = new Quest.Services.Dbio.MasterPricing.FilterLogs();
                BufferMgr.TransferBuffer(filterLog, _filterLog);
                dbContext.FilterLogs.Add(_filterLog);
                dbContext.SaveChanges();
                if (_filterLog.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Quest.Functional.Logging.FilterLog not created"));
                }
                filterLogId = new FilterLogId(_filterLog.Id);
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus read(MasterPricingEntities dbContext, Quest.Functional.Logging.FilterLogId filterLogId, out Quest.Services.Dbio.MasterPricing.FilterLogs filterLog)
        {
            // Initialize
            filterLog = null;


            try
            {
                filterLog = dbContext.FilterLogs.Where(r => r.Id == filterLogId.Id).SingleOrDefault();
                if (filterLog == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", filterLogId.Id))));
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
        private questStatus read(MasterPricingEntities dbContext, string username, out Quest.Services.Dbio.MasterPricing.FilterLogs filterLog)
        {
            // Initialize
            filterLog = null;


            try
            {
                filterLog = dbContext.FilterLogs.Where(r => r.Username == username).SingleOrDefault();
                if (filterLog == null)
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
        private questStatus update(MasterPricingEntities dbContext, Quest.Functional.Logging.FilterLog filterLog)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                FilterLogId filterLogId = new FilterLogId(filterLog.Id);
                Quest.Services.Dbio.MasterPricing.FilterLogs _filterLog = null;
                status = read(dbContext, filterLogId, out _filterLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(filterLog, _filterLog);
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
        private questStatus delete(MasterPricingEntities dbContext, Quest.Functional.Logging.FilterLogId filterLogId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                Quest.Services.Dbio.MasterPricing.FilterLogs _filterLog = null;
                status = read(dbContext, filterLogId, out _filterLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.FilterLogs.Remove(_filterLog);
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
