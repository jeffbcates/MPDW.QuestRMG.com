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
using Quest.MPDW.Services.Data;
using Quest.Services.Dbio.FMS;


namespace Quest.MPDW.Services.Data.Accounts
{
    public class DbUserPrivilegesMgr : DbMgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbPrivilegesMgr _dbPrivilegesMgr = null;
        private DbUsersMgr _dbUsersMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public DbUserPrivilegesMgr(UserSession userSession)
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
        public questStatus Create(UserPrivilege userPrivilege, out UserPrivilegeId userPrivilegeId)
        {
            // Initialize
            questStatus status = null;
            userPrivilegeId = null;


            // Data rules.
            userPrivilege.Created = DateTime.Now;


            // Create the userPrivilege
            using (FMSEntities dbContext = new FMSEntities())
            {
                status = create(dbContext, userPrivilege, out userPrivilegeId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, UserPrivilege userPrivilege, out UserPrivilegeId userPrivilegeId)
        {
            // Initialize
            questStatus status = null;
            userPrivilegeId = null;


            // Data rules.
            userPrivilege.Created = DateTime.Now;


            // Create the userPrivilege in this transaction.
            status = create((FMSEntities)trans.DbContext, userPrivilege, out userPrivilegeId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(UserPrivilegeId userPrivilegeId, out UserPrivilege userPrivilege)
        {
            // Initialize
            questStatus status = null;
            userPrivilege = null;


            // Perform read
            using (FMSEntities dbContext = new FMSEntities())
            {
                Quest.Services.Dbio.FMS.UserPrivileges _userPrivileges = null;
                status = read(dbContext, userPrivilegeId, out _userPrivileges);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                userPrivilege = new UserPrivilege();
                BufferMgr.TransferBuffer(_userPrivileges, userPrivilege);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, UserPrivilegeId userPrivilegeId, out UserPrivilege userPrivilege)
        {
            // Initialize
            questStatus status = null;
            userPrivilege = null;


            // Perform read
            Quest.Services.Dbio.FMS.UserPrivileges _userPrivileges = null;
            status = read((FMSEntities)trans.DbContext, userPrivilegeId, out _userPrivileges);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            userPrivilege = new UserPrivilege();
            BufferMgr.TransferBuffer(_userPrivileges, userPrivilege);

            return (new questStatus(Severity.Success));
        }
        public questStatus Read(UserId userId, out UserPrivilegeList userPrivilegeList)
        {
            // Initialize
            questStatus status = null;
            userPrivilegeList = null;


            // Get user
            User user = null;
            status = _dbUsersMgr.Read(userId, out user);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get user privileges
            using (FMSEntities dbContext = new FMSEntities())
            {
                List<Quest.Services.Dbio.FMS.UserPrivileges> _userPrivilegeList = null;
                status = read(dbContext, userId, out _userPrivilegeList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                userPrivilegeList = new UserPrivilegeList();
                userPrivilegeList.User = user;
                foreach (Quest.Services.Dbio.FMS.UserPrivileges _userPrivilege in _userPrivilegeList)
                {
                    // Get privilege
                    PrivilegeId privilegeId = new PrivilegeId(_userPrivilege.PrivilegeId);
                    Privilege privilege = null;
                    status = _dbPrivilegesMgr.Read(privilegeId, out privilege);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (status);
                    }
                    userPrivilegeList.PrivilegeList.Add(privilege);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, UserId userId, out UserPrivilegeList userPrivilegeList)
        {
            // Initialize
            questStatus status = null;
            userPrivilegeList = null;


            // Get user
            User user = null;
            status = _dbUsersMgr.Read(userId, out user);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get user privileges
            List<Quest.Services.Dbio.FMS.UserPrivileges> _userPrivilegeList = null;
            status = read((FMSEntities)trans.DbContext, userId, out _userPrivilegeList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            userPrivilegeList = new UserPrivilegeList();
            userPrivilegeList.User = user;
            foreach (Quest.Services.Dbio.FMS.UserPrivileges _userPrivilege in _userPrivilegeList)
            {
                // Get privilege
                PrivilegeId privilegeId = new PrivilegeId(_userPrivilege.PrivilegeId);
                Privilege privilege = null;
                status = _dbPrivilegesMgr.Read(privilegeId, out privilege);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                userPrivilegeList.PrivilegeList.Add(privilege);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(PrivilegeId privilegeId, out PrivilegeUserList privilegeUserList)
        {
            // Initialize
            questStatus status = null;
            privilegeUserList = null;


            // Get privilege
            Privilege privilege = null;
            status = _dbPrivilegesMgr.Read(privilegeId, out privilege);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get privilege users
            using (FMSEntities dbContext = new FMSEntities())
            {
                List<Quest.Services.Dbio.FMS.UserPrivileges> _userPrivilegesList = null;
                status = read(dbContext, privilegeId, out _userPrivilegesList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                privilegeUserList = new PrivilegeUserList();
                privilegeUserList.Privilege = privilege;
                foreach (Quest.Services.Dbio.FMS.UserPrivileges _userPrivileges in _userPrivilegesList)
                {
                    // Get user
                    UserId userId = new UserId(_userPrivileges.PrivilegeId);
                    User user = null;
                    status = _dbUsersMgr.Read(userId, out user);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (status);
                    }
                    privilegeUserList.UserList.Add(user);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, PrivilegeId privilegeId, out PrivilegeUserList privilegeUserList)
        {
            // Initialize
            questStatus status = null;
            privilegeUserList = null;


            // Get privilege
            Privilege privilege = null;
            status = _dbPrivilegesMgr.Read(privilegeId, out privilege);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get privilege users
            List<Quest.Services.Dbio.FMS.UserPrivileges> _userPrivilegesList = null;
            status = read((FMSEntities)trans.DbContext, privilegeId, out _userPrivilegesList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            privilegeUserList = new PrivilegeUserList();
            privilegeUserList.Privilege = privilege;
            foreach (Quest.Services.Dbio.FMS.UserPrivileges _userPrivileges in _userPrivilegesList)
            {
                // Get user
                UserId userId = new UserId(_userPrivileges.PrivilegeId);
                User user = null;
                status = _dbUsersMgr.Read(userId, out user);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                privilegeUserList.UserList.Add(user);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(UserPrivilege userPrivilege)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (FMSEntities dbContext = new FMSEntities())
            {
                status = update(dbContext, userPrivilege);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, UserPrivilege userPrivilege)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((FMSEntities)trans.DbContext, userPrivilege);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(UserPrivilegeId userPrivilegeId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (FMSEntities dbContext = new FMSEntities())
            {
                status = delete(dbContext, userPrivilegeId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, UserPrivilegeId userPrivilegeId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((FMSEntities)trans.DbContext, userPrivilegeId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(PrivilegeId privilegeId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (FMSEntities dbContext = new FMSEntities())
            {
                status = delete(dbContext, privilegeId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, PrivilegeId privilegeId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((FMSEntities)trans.DbContext, privilegeId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(UserId userId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (FMSEntities dbContext = new FMSEntities())
            {
                status = delete(dbContext, userId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, UserId userId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((FMSEntities)trans.DbContext, userId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<UserPrivilege> userPrivilegeList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            userPrivilegeList = null;
            queryResponse = null;


            using (FMSEntities dbContext = new FMSEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.FMS.UserPrivileges).GetProperties().ToArray();
                        List<Quest.Services.Dbio.FMS.UserPrivileges> _userPrivilegesList = dbContext.UserPrivileges.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_userPrivilegesList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        userPrivilegeList = new List<UserPrivilege>();
                        foreach (Quest.Services.Dbio.FMS.UserPrivileges _userPrivileges in _userPrivilegesList)
                        {
                            UserPrivilege userPrivilege = new UserPrivilege();
                            BufferMgr.TransferBuffer(_userPrivileges, userPrivilege);
                            userPrivilegeList.Add(userPrivilege);
                        }
                        status = BuildQueryResponse(_userPrivilegesList.Count, queryOptions, out queryResponse);
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
                _dbPrivilegesMgr = new DbPrivilegesMgr(this.UserSession);
                _dbUsersMgr = new DbUsersMgr(this.UserSession);
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                throw new System.Exception(status.Message, ex);
            }
            return (new questStatus(Severity.Success));
        }


        #region UserPrivileges
        /*----------------------------------------------------------------------------------------------------------------------------------
         * UserPrivileges
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(FMSEntities dbContext, UserPrivilege userPrivilege, out UserPrivilegeId userPrivilegeId)
        {
            // Initialize
            userPrivilegeId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.FMS.UserPrivileges _userPrivileges = new Quest.Services.Dbio.FMS.UserPrivileges();
                _userPrivileges.PrivilegeId = userPrivilege.Privilege.Id;
                _userPrivileges.UserId = userPrivilege.User.Id;
                _userPrivileges.Created = DateTime.Now;
                dbContext.UserPrivileges.Add(_userPrivileges);
                dbContext.SaveChanges();
                if (_userPrivileges.Id == 0)
                {
                    return (new questStatus(Severity.Error, "UserPrivilege not created"));
                }
                userPrivilegeId = new UserPrivilegeId(_userPrivileges.Id);
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus read(FMSEntities dbContext, UserPrivilegeId userPrivilegeId, out Quest.Services.Dbio.FMS.UserPrivileges userPrivilege)
        {
            // Initialize
            userPrivilege = null;


            try
            {
                userPrivilege = dbContext.UserPrivileges.Where(r => r.Id == userPrivilegeId.Id).SingleOrDefault();
                if (userPrivilege == null)
                {
                    return (new questStatus(Severity.Warning, String.Format("WARNING: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", userPrivilegeId.Id))));
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
        private questStatus read(FMSEntities dbContext, UserId userId, out List<Quest.Services.Dbio.FMS.UserPrivileges> userPrivilegeList)
        {
            // Initialize
            userPrivilegeList = null;


            try
            {
                userPrivilegeList = dbContext.UserPrivileges.Where(r => r.UserId == userId.Id).ToList();
                if (userPrivilegeList == null)
                {
                    return (new questStatus(Severity.Warning, String.Format("WARNING: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("UserId {0} not found", userId.Id))));
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
        private questStatus read(FMSEntities dbContext, PrivilegeId privilegeId, out List<Quest.Services.Dbio.FMS.UserPrivileges> userPrivilegeList)
        {
            // Initialize
            userPrivilegeList = null;


            try
            {
                userPrivilegeList = dbContext.UserPrivileges.Where(r => r.PrivilegeId == privilegeId.Id).ToList();
                if (userPrivilegeList == null)
                {
                    return (new questStatus(Severity.Warning, String.Format("WARNING: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("PrivilegeId {0} not found", privilegeId.Id))));
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
        private questStatus update(FMSEntities dbContext, UserPrivilege userPrivilege)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                UserPrivilegeId userPrivilegeId = new UserPrivilegeId(userPrivilege.Id);
                Quest.Services.Dbio.FMS.UserPrivileges _userPrivileges = null;
                status = read(dbContext, userPrivilegeId, out _userPrivileges);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(userPrivilege, _userPrivileges);
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
        private questStatus delete(FMSEntities dbContext, UserPrivilegeId userPrivilegeId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                Quest.Services.Dbio.FMS.UserPrivileges _userPrivileges = null;
                status = read(dbContext, userPrivilegeId, out _userPrivileges);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.UserPrivileges.Remove(_userPrivileges);
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
        private questStatus delete(FMSEntities dbContext, UserId userId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                List<Quest.Services.Dbio.FMS.UserPrivileges> userPrivilegeList = null;
                status = read(dbContext, userId, out userPrivilegeList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the records.
                foreach (Quest.Services.Dbio.FMS.UserPrivileges userPrivilege in userPrivilegeList)
                {
                    dbContext.UserPrivileges.Remove(userPrivilege);
                }
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
        private questStatus delete(FMSEntities dbContext, PrivilegeId privilegeId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                List<Quest.Services.Dbio.FMS.UserPrivileges> userPrivilegeList = null;
                status = read(dbContext, privilegeId, out userPrivilegeList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the records.
                foreach (Quest.Services.Dbio.FMS.UserPrivileges userPrivilege in userPrivilegeList)
                {
                    dbContext.UserPrivileges.Remove(userPrivilege);
                }
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
