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
    public class DbPortalRequestLogsMgr : DbMgrSessionBased
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
        public DbPortalRequestLogsMgr(UserSession userSession)
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
        public questStatus Create(Quest.Functional.Logging.PortalRequestLog portalRequestLog, out Quest.Functional.Logging.PortalRequestLogId portalRequestLogId)
        {
            // Initialize
            questStatus status = null;
            portalRequestLogId = null;


            // Data rules.


            // Create the portalRequestLog
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = create(dbContext, portalRequestLog, out portalRequestLogId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Quest.Functional.Logging.PortalRequestLog portalRequestLog, out Quest.Functional.Logging.PortalRequestLogId portalRequestLogId)
        {
            // Initialize
            questStatus status = null;
            portalRequestLogId = null;


            // Data rules.


            // Create the portalRequestLog in this transaction.
            status = create((MasterPricingEntities)trans.DbContext, portalRequestLog, out portalRequestLogId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(Quest.Functional.Logging.PortalRequestLogId portalRequestLogId, out Quest.Functional.Logging.PortalRequestLog portalRequestLog)
        {
            // Initialize
            questStatus status = null;
            portalRequestLog = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.PortalRequestLogs _portalRequestLog = null;
                status = read(dbContext, portalRequestLogId, out _portalRequestLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                portalRequestLog = new Quest.Functional.Logging.PortalRequestLog();
                BufferMgr.TransferBuffer(_portalRequestLog, portalRequestLog);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(string username, out Quest.Functional.Logging.PortalRequestLog portalRequestLog)
        {
            // Initialize
            questStatus status = null;
            portalRequestLog = null;


            // Perform read
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.PortalRequestLogs _portalRequestLog = null;
                status = read(dbContext, username, out _portalRequestLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                portalRequestLog = new Quest.Functional.Logging.PortalRequestLog();
                BufferMgr.TransferBuffer(_portalRequestLog, portalRequestLog);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, Quest.Functional.Logging.PortalRequestLogId portalRequestLogId, out Quest.Functional.Logging.PortalRequestLog portalRequestLog)
        {
            // Initialize
            questStatus status = null;
            portalRequestLog = null;


            // Perform read.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                Quest.Services.Dbio.MasterPricing.PortalRequestLogs _portalRequestLog = null;
                status = read((MasterPricingEntities)trans.DbContext, portalRequestLogId, out _portalRequestLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                portalRequestLog = new Quest.Functional.Logging.PortalRequestLog();
                BufferMgr.TransferBuffer(_portalRequestLog, portalRequestLog);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Quest.Functional.Logging.PortalRequestLog portalRequestLog)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = update(dbContext, portalRequestLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Quest.Functional.Logging.PortalRequestLog portalRequestLog)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((MasterPricingEntities)trans.DbContext, portalRequestLog);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(Quest.Functional.Logging.PortalRequestLogId portalRequestLogId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                status = delete(dbContext, portalRequestLogId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, Quest.Functional.Logging.PortalRequestLogId portalRequestLogId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((MasterPricingEntities)trans.DbContext, portalRequestLogId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Quest.Functional.Logging.PortalRequestLog> portalRequestLogList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            portalRequestLogList = null;
            queryResponse = null;


            using (MasterPricingEntities dbContext = new MasterPricingEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.MasterPricing.PortalRequestLogs).GetProperties().ToArray();
                        int totalRecords = dbContext.PortalRequestLogs.Where(BuildWhereClause(queryOptions, dbProperties)).Count();
                        List<Quest.Services.Dbio.MasterPricing.PortalRequestLogs> _portalRequestLogsList = dbContext.PortalRequestLogs.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_portalRequestLogsList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        portalRequestLogList = new List<Quest.Functional.Logging.PortalRequestLog>();
                        foreach (Quest.Services.Dbio.MasterPricing.PortalRequestLogs _portalRequestLog in _portalRequestLogsList)
                        {
                            Quest.Functional.Logging.PortalRequestLog portalRequestLog = new Quest.Functional.Logging.PortalRequestLog();
                            BufferMgr.TransferBuffer(_portalRequestLog, portalRequestLog);
                            portalRequestLogList.Add(portalRequestLog);
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
                    dbContext.PortalRequestLogs.RemoveRange(dbContext.PortalRequestLogs.Where(r => r.Id > 0));
                    dbContext.SaveChanges();
                }
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: clearing Portal Requests Log {0}", ex.Message)));
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


        #region PortalRequestLogs
        /*----------------------------------------------------------------------------------------------------------------------------------
         * PortalRequestLogs
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(MasterPricingEntities dbContext, Quest.Functional.Logging.PortalRequestLog portalRequestLog, out PortalRequestLogId portalRequestLogId)
        {
            // Initialize
            portalRequestLogId = null;


            // Initialize
            portalRequestLog.UserSessionId = this.UserSession.Id;
            portalRequestLog.Username = this.UserSession.User.Username;
            portalRequestLog.Created = DateTime.Now;


            // Perform create
            try
            {
                Quest.Services.Dbio.MasterPricing.PortalRequestLogs _portalRequestLog = new Quest.Services.Dbio.MasterPricing.PortalRequestLogs();
                BufferMgr.TransferBuffer(portalRequestLog, _portalRequestLog);
                dbContext.PortalRequestLogs.Add(_portalRequestLog);
                dbContext.SaveChanges();
                if (_portalRequestLog.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Quest.Functional.Logging.PortalLog not created"));
                }
                portalRequestLogId = new PortalRequestLogId(_portalRequestLog.Id);
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus read(MasterPricingEntities dbContext, Quest.Functional.Logging.PortalRequestLogId portalRequestLogId, out Quest.Services.Dbio.MasterPricing.PortalRequestLogs portalRequestLog)
        {
            // Initialize
            portalRequestLog = null;


            try
            {
                portalRequestLog = dbContext.PortalRequestLogs.Where(r => r.Id == portalRequestLogId.Id).SingleOrDefault();
                if (portalRequestLog == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", portalRequestLogId.Id))));
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
        private questStatus read(MasterPricingEntities dbContext, string username, out Quest.Services.Dbio.MasterPricing.PortalRequestLogs portalRequestLog)
        {
            // Initialize
            portalRequestLog = null;


            try
            {
                portalRequestLog = dbContext.PortalRequestLogs.Where(r => r.Username == username).SingleOrDefault();
                if (portalRequestLog == null)
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
        private questStatus update(MasterPricingEntities dbContext, Quest.Functional.Logging.PortalRequestLog portalRequestLog)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                PortalRequestLogId portalRequestLogId = new PortalRequestLogId(portalRequestLog.Id);
                Quest.Services.Dbio.MasterPricing.PortalRequestLogs _portalRequestLog = null;
                status = read(dbContext, portalRequestLogId, out _portalRequestLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(portalRequestLog, _portalRequestLog);
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
        private questStatus delete(MasterPricingEntities dbContext, Quest.Functional.Logging.PortalRequestLogId portalRequestLogId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                Quest.Services.Dbio.MasterPricing.PortalRequestLogs _portalRequestLog = null;
                status = read(dbContext, portalRequestLogId, out _portalRequestLog);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.PortalRequestLogs.Remove(_portalRequestLog);
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
