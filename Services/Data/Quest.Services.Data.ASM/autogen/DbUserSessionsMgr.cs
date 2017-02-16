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
using Quest.Util.Encryption;
using Quest.Functional.ASM;
using Quest.MPDW.Services.Data;
using Quest.Services.Dbio.FMS;


namespace Quest.MPDW.Services.Data.Accounts
{
    public class DbUserSessionsMgr : DbMgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        AESEncryption _aesEncryption = null;
        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public DbUserSessionsMgr(UserSession userSession)
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
        public questStatus Create(UserSession userSession, out UserSessionId userSessionId)
        {
            // Initialize
            questStatus status = null;
            userSessionId = null;


            // Data rules.
            userSession.Created = DateTime.Now;


            // Create the user session
            using (FMSEntities dbContext = new FMSEntities())
            {
                status = create(dbContext, userSession, out userSessionId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, UserSession userSession, out UserSessionId userSessionId)
        {
            // Initialize
            questStatus status = null;
            userSessionId = null;


            // Data rules.
            userSession.Created = DateTime.Now;


            // Create the user session in this transaction.
            status = create((FMSEntities)trans.DbContext, userSession, out userSessionId);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(UserSessionId userSessionId, out UserSession userSession)
        {
            // Initialize
            questStatus status = null;
            userSession = null;


            // Perform read
            using (FMSEntities dbContext = new FMSEntities())
            {
                Quest.Services.Dbio.FMS.UserSessions _userSession = null;
                status = read(dbContext, userSessionId, out _userSession);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                userSession = new UserSession();
                BufferMgr.TransferBuffer(_userSession, userSession);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, UserSessionId userSessionId, out UserSession userSession)
        {
            // Initialize
            questStatus status = null;
            userSession = null;


            // Perform read.
            using (FMSEntities dbContext = new FMSEntities())
            {
                Quest.Services.Dbio.FMS.UserSessions _userSession = null;
                status = read((FMSEntities)trans.DbContext, userSessionId, out _userSession);
                if (! questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                userSession = new UserSession();
                BufferMgr.TransferBuffer(_userSession, userSession);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(UserSession userSession)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (FMSEntities dbContext = new FMSEntities())
            {
                status = update(dbContext, userSession);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, UserSession userSession)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((FMSEntities)trans.DbContext, userSession);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(UserSessionId userSessionId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (FMSEntities dbContext = new FMSEntities())
            {
                status = delete(dbContext, userSessionId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, UserSessionId userSessionId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((FMSEntities)trans.DbContext, userSessionId);
            if (! questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<UserSession> userSessionList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            userSessionList = null;
            queryResponse = null;


            using (FMSEntities dbContext = new FMSEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.FMS.UserSessions).GetProperties().ToArray();
                        List<Quest.Services.Dbio.FMS.UserSessions> _userSessionsList = dbContext.UserSessions.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_userSessionsList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        userSessionList = new List<UserSession>();
                        foreach (Quest.Services.Dbio.FMS.UserSessions _userSession in _userSessionsList)
                        {
                            UserSession userSession = new UserSession();
                            BufferMgr.TransferBuffer(_userSession, userSession);
                            userSessionList.Add(userSession);
                        }
                        status = BuildQueryResponse(_userSessionsList.Count, queryOptions, out queryResponse);
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
                _aesEncryption = new AESEncryption();
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                throw new System.Exception(status.Message, ex);
            }
            return (new questStatus(Severity.Success));
        }


        #region UserSessions
        /*----------------------------------------------------------------------------------------------------------------------------------
         * UserSessions
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(FMSEntities dbContext, UserSession userSession, out UserSessionId userSessionId)
        {
            // Initialize
            userSessionId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.FMS.UserSessions _userSession = new Quest.Services.Dbio.FMS.UserSessions();
                BufferMgr.TransferBuffer(userSession, _userSession);
                dbContext.UserSessions.Add(_userSession);
                dbContext.SaveChanges();
                if (_userSession.Id == 0)
                {
                    return (new questStatus(Severity.Error, "UserSession not created"));
                }
                userSessionId = new UserSessionId(_userSession.Id);
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
        private questStatus read(FMSEntities dbContext, UserSessionId userSessionId, out Quest.Services.Dbio.FMS.UserSessions userSession)
        {
            // Initialize
            userSession = null;


            try
            {
                userSession = dbContext.UserSessions.Where(r => r.Id == userSessionId.Id).SingleOrDefault();
                if (userSession == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", userSessionId.Id))));
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
        private questStatus update(FMSEntities dbContext, UserSession userSession)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                UserSessionId userSessionId = new UserSessionId(userSession.Id);
                Quest.Services.Dbio.FMS.UserSessions _userSession = null;
                status = read(dbContext, userSessionId, out _userSession);
                if (! questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(userSession, _userSession);
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
        private questStatus delete(FMSEntities dbContext, UserSessionId userSessionId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                Quest.Services.Dbio.FMS.UserSessions _userSession = null;
                status = read(dbContext, userSessionId, out _userSession);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.UserSessions.Remove(_userSession);
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
