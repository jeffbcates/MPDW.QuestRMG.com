using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
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
using Quest.Functional.Logging;
using Quest.MPDW.Services.Data;
using Quest.Services.Dbio.PWTrackerLogging;


namespace Quest.Services.Data.Logging
{
    public class DbLogMgr : DbMgr
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private UserSession _userSession = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public DbLogMgr()
            : base()
        {
            initialize();
        }
        public DbLogMgr(UserSession userSession)
            : base()
        {
            _userSession = userSession;
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
        public questStatus Log(questStatus status)
        {
            int userSessionId = BaseId.INVALID_ID;
            int severity = (int)status.Severity;
            string module = status.Module + '.' + status.Method;
            string message = status.Message;

            return Log(userSessionId, severity, module, message);
        }
        public questStatus Log(UserSession userSession, questStatus status)
        {
            int userSessionId = userSession == null ? BaseId.INVALID_ID : userSession.Id;
            int severity = (int)status.Severity;
            string module = status.Module + '.' + status.Method;
            string message = status.Message;

            return Log(userSessionId, severity, module, message);
        }
        public questStatus Log(int userSessionId, int severity, string module, string message)
        {
            // Initialize
            questStatus status = null;
            LogId logId = null;

            Log log = new Log();
            log.UserSessionId = userSessionId;
            log.Severity = severity;
            log.Module = module;
            log.Message = message;
            log.Created = DateTime.Now;

            // Create the emailRecipient
            using (PWTrackerLoggingEntities dbContext = new PWTrackerLoggingEntities())
            {
                status = create(dbContext, log, out logId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }

            return (new questStatus(Severity.Success));
        }

        #region CRUD
        /*----------------------------------------------------------------------------------------------------------------------------------
         * CRUD
         *---------------------------------------------------------------------------------------------------------------------------------*/
        public questStatus Create(Log log, out LogId logId)
        {
            // Initialize
            questStatus status = null;
            logId = null;

            // Data rules.
            log.Created = DateTime.Now;

            // Create the log
            using (PWTrackerLoggingEntities dbContext = new PWTrackerLoggingEntities())
            {
                status = create(dbContext, log, out logId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Log log, out LogId logId)
        {
            // Initialize
            questStatus status = null;
            logId = null;

            // Data rules.
            log.Created = DateTime.Now;


            // Create the log in this transaction.
            status = create((PWTrackerLoggingEntities)trans.DbContext, log, out logId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(LogId logId, out Log log)
        {
            // Initialize
            questStatus status = null;
            log = null;


            // Perform read
            using (PWTrackerLoggingEntities dbContext = new PWTrackerLoggingEntities())
            {
                Quest.Services.Dbio.PWTrackerLogging.Logs _log = null;
                status = read(dbContext, logId, out _log);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                log = new Log();
                BufferMgr.TransferBuffer(_log, log);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, LogId logId, out Log log)
        {
            // Initialize
            questStatus status = null;
            log = null;


            // Perform read.
            using (PWTrackerLoggingEntities dbContext = new PWTrackerLoggingEntities())
            {
                Quest.Services.Dbio.PWTrackerLogging.Logs _log = null;
                status = read((PWTrackerLoggingEntities)trans.DbContext, logId, out _log);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                log = new Log();
                BufferMgr.TransferBuffer(_log, log);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Log> logList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            logList = null;
            queryResponse = null;


            using (PWTrackerLoggingEntities dbContext = new PWTrackerLoggingEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.PWTrackerLogging.Logs).GetProperties().ToArray();
                        List<Quest.Services.Dbio.PWTrackerLogging.Logs> _logsList = dbContext.Logs.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_logsList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        logList = new List<Log>();
                        foreach (Quest.Services.Dbio.PWTrackerLogging.Logs _log in _logsList)
                        {
                            Log log = new Log();
                            BufferMgr.TransferBuffer(_log, log);
                            logList.Add(log);
                        }
                        status = BuildQueryResponse(_logsList.Count, queryOptions, out queryResponse);
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
                if (_userSession == null)
                {
                    _userSession = new UserSession();
                    _userSession.Id = BaseId.INVALID_ID;
                    _userSession.UserId = BaseId.INVALID_ID;
                }
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                throw new System.Exception(status.Message, ex);
            }
            return (new questStatus(Severity.Success));
        }


        #region Logs
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Logs
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(PWTrackerLoggingEntities dbContext, Log log, out LogId logId)
        {
            // Initialize
            logId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.PWTrackerLogging.Logs _log = new Quest.Services.Dbio.PWTrackerLogging.Logs();
                BufferMgr.TransferBuffer(log, _log);
                dbContext.Logs.Add(_log);
                dbContext.SaveChanges();
                if (_log.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Log not created"));
                }
                logId = new LogId(_log.Id);
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);

                String fullErrorMessage = string.Join("; ", errorMessages);
                String exceptionMessage = string.Concat(ex.Message, " The DbEntityValidationException errors are: ", fullErrorMessage);
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                                        exceptionMessage)));
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus read(PWTrackerLoggingEntities dbContext, LogId logId, out Quest.Services.Dbio.PWTrackerLogging.Logs log)
        {
            // Initialize
            log = null;


            try
            {
                log = dbContext.Logs.Where(r => r.Id == logId.Id).SingleOrDefault();
                if (log == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", logId.Id))));
                }
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);

                String fullErrorMessage = string.Join("; ", errorMessages);
                String exceptionMessage = string.Concat(ex.Message, " The DbEntityValidationException errors are: ", fullErrorMessage);
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                                        exceptionMessage)));
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus update(PWTrackerLoggingEntities dbContext, Log log)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                LogId logId = new LogId(log.Id);
                Quest.Services.Dbio.PWTrackerLogging.Logs _log = null;
                status = read(dbContext, logId, out _log);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(log, _log);
                dbContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);

                String fullErrorMessage = string.Join("; ", errorMessages);
                String exceptionMessage = string.Concat(ex.Message, " The DbEntityValidationException errors are: ", fullErrorMessage);
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                                        exceptionMessage)));
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus delete(PWTrackerLoggingEntities dbContext, LogId logId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                Quest.Services.Dbio.PWTrackerLogging.Logs _log = null;
                status = read(dbContext, logId, out _log);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.Logs.Remove(_log);
                dbContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);

                String fullErrorMessage = string.Join("; ", errorMessages);
                String exceptionMessage = string.Concat(ex.Message, " The DbEntityValidationException errors are: ", fullErrorMessage);
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                                        exceptionMessage)));
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
