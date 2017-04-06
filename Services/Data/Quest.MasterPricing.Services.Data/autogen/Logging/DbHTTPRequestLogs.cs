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


namespace Quest.MasterPricing.Services.Data.Logging
{
    public class DbHTTPRequestLogsMgr : DbMgrSessionBased
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
        public DbHTTPRequestLogsMgr(UserSession userSession)
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
        public questStatus Create(Quest.Functional.Logging.HTTPRequestLog httpRequestLog, out Quest.Functional.Logging.HTTPRequestLogId httpRequestLogId)
        {
            // Initialize
            questStatus status = null;
            httpRequestLogId = null;


            // Data rules.


            // Create the httpRequestLog
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = create(dbContext, httpRequestLog, out httpRequestLogId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.Logging.HTTPRequestLog httpRequestLog, out Quest.Functional.Logging.HTTPRequestLogId httpRequestLogId)
        {
            // Initialize
            questStatus status = null;
            httpRequestLogId = null;


            // Data rules.


            // Create the httpRequestLog in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, httpRequestLog, out httpRequestLogId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(Quest.Functional.Logging.HTTPRequestLogId httpRequestLogId, out Quest.Functional.Logging.HTTPRequestLog httpRequestLog)
        {
            // Initialize
            questStatus status = null;
            httpRequestLog = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.HTTPRequestLogs _httpRequestLog = null;
                status = read(dbContext, httpRequestLogId, out _httpRequestLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                httpRequestLog = new Quest.Functional.Logging.HTTPRequestLog();
                BufferMgr.TransferBuffer(_httpRequestLog, httpRequestLog);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(string username, out Quest.Functional.Logging.HTTPRequestLog httpRequestLog)
        {
            // Initialize
            questStatus status = null;
            httpRequestLog = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.HTTPRequestLogs _httpRequestLog = null;
                status = read(dbContext, username, out _httpRequestLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                httpRequestLog = new Quest.Functional.Logging.HTTPRequestLog();
                BufferMgr.TransferBuffer(_httpRequestLog, httpRequestLog);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, Quest.Functional.Logging.HTTPRequestLogId httpRequestLogId, out Quest.Functional.Logging.HTTPRequestLog httpRequestLog)
        {
            // Initialize
            questStatus status = null;
            httpRequestLog = null;


            // Perform read.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.HTTPRequestLogs _httpRequestLog = null;
                status = read((MasterPricingEntities)trans.DbContext, httpRequestLogId, out _httpRequestLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                httpRequestLog = new Quest.Functional.Logging.HTTPRequestLog();
                BufferMgr.TransferBuffer(_httpRequestLog, httpRequestLog);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.Logging.HTTPRequestLog httpRequestLog)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = update(dbContext, httpRequestLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Quest.Functional.Logging.HTTPRequestLog httpRequestLog)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((MasterPricingEntities)trans.DbContext, httpRequestLog);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(Quest.Functional.Logging.HTTPRequestLogId httpRequestLogId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, httpRequestLogId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, Quest.Functional.Logging.HTTPRequestLogId httpRequestLogId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, httpRequestLogId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.Logging.HTTPRequestLog> httpRequestLogList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            httpRequestLogList = null;
            queryResponse = null;


            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.MasterPricing.HTTPRequestLogs).GetProperties().ToArray();
                        int totalRecords = dbContext.HTTPRequestLogs.Where(BuildWhereClause(queryOptions, dbProperties)).Count();
                        List<Quest.Services.Dbio.MasterPricing.HTTPRequestLogs> _httpRequestLogsList = dbContext.HTTPRequestLogs.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_httpRequestLogsList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        httpRequestLogList = new List<Quest.Functional.Logging.HTTPRequestLog>();
                        foreach (Quest.Services.Dbio.MasterPricing.HTTPRequestLogs _httpRequestLog in _httpRequestLogsList)
                        {
                            Quest.Functional.Logging.HTTPRequestLog httpRequestLog = new Quest.Functional.Logging.HTTPRequestLog();
                            BufferMgr.TransferBuffer(_httpRequestLog, httpRequestLog);
                            httpRequestLogList.Add(httpRequestLog);
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


        #region HTTPRequestLogs
        /*----------------------------------------------------------------------------------------------------------------------------------
         * HTTPRequestLogs
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(MasterPricingEntities dbContext, Quest.Functional.Logging.HTTPRequestLog httpRequestLog, out HTTPRequestLogId httpRequestLogId)
        {
            // Initialize
            httpRequestLogId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.MasterPricing.HTTPRequestLogs _httpRequestLog = new Quest.Services.Dbio.MasterPricing.HTTPRequestLogs();
                BufferMgr.TransferBuffer(httpRequestLog, _httpRequestLog);
                dbContext.HTTPRequestLogs.Add(_httpRequestLog);
                dbContext.SaveChanges();
                if (_httpRequestLog.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Quest.Functional.Logging.HTTPRequestLog not created"));
                }
                httpRequestLogId = new HTTPRequestLogId(_httpRequestLog.Id);
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus read(MasterPricingEntities dbContext, Quest.Functional.Logging.HTTPRequestLogId httpRequestLogId, out Quest.Services.Dbio.MasterPricing.HTTPRequestLogs httpRequestLog)
        {
            // Initialize
            httpRequestLog = null;


            try
            {
                httpRequestLog = dbContext.HTTPRequestLogs.Where(r => r.Id == httpRequestLogId.Id).SingleOrDefault();
                if (httpRequestLog == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", httpRequestLogId.Id))));
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
        private questStatus read(MasterPricingEntities dbContext, string username, out Quest.Services.Dbio.MasterPricing.HTTPRequestLogs httpRequestLog)
        {
            // Initialize
            httpRequestLog = null;


            try
            {
                httpRequestLog = dbContext.HTTPRequestLogs.Where(r => r.Username == username).SingleOrDefault();
                if (httpRequestLog == null)
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
        private questStatus update(MasterPricingEntities dbContext, Quest.Functional.Logging.HTTPRequestLog httpRequestLog)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                HTTPRequestLogId httpRequestLogId = new HTTPRequestLogId(httpRequestLog.Id);
                Quest.Services.Dbio.MasterPricing.HTTPRequestLogs _httpRequestLog = null;
                status = read(dbContext, httpRequestLogId, out _httpRequestLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(httpRequestLog, _httpRequestLog);
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
        private questStatus delete(MasterPricingEntities dbContext, Quest.Functional.Logging.HTTPRequestLogId httpRequestLogId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                Quest.Services.Dbio.MasterPricing.HTTPRequestLogs _httpRequestLog = null;
                status = read(dbContext, httpRequestLogId, out _httpRequestLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.HTTPRequestLogs.Remove(_httpRequestLog);
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
