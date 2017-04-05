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
    public class DbGroupUsersMgr : DbMgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbGroupsMgr _dbGroupsMgr = null;
        private DbUsersMgr _dbUsersMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public DbGroupUsersMgr(UserSession userSession)
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
        public questStatus Create(GroupUser groupUser, out GroupUserId groupUserId)
        {
            // Initialize
            questStatus status = null;
            groupUserId = null;


            // Data rules.
            groupUser.Created = DateTime.Now;


            // Create the groupUser
            using (FMSEntities dbContext = new FMSEntities())
            {
                status = create(dbContext, groupUser, out groupUserId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, GroupUser groupUser, out GroupUserId groupUserId)
        {
            // Initialize
            questStatus status = null;
            groupUserId = null;


            // Data rules.
            groupUser.Created = DateTime.Now;


            // Create the groupUser in this transaction.
            status = create((FMSEntities)trans.DbContext, groupUser, out groupUserId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(GroupUserId groupUserId, out GroupUser groupUser)
        {
            // Initialize
            questStatus status = null;
            groupUser = null;


            // Perform read
            using (FMSEntities dbContext = new FMSEntities())
            {
                Quest.Services.Dbio.FMS.GroupUsers _groupUser = null;
                status = read(dbContext, groupUserId, out _groupUser);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                groupUser = new GroupUser();
                BufferMgr.TransferBuffer(_groupUser, groupUser);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, GroupUserId groupUserId, out GroupUser groupUser)
        {
            // Initialize
            questStatus status = null;
            groupUser = null;


            // Perform read
            Quest.Services.Dbio.FMS.GroupUsers _groupUser = null;
            status = read((FMSEntities)trans.DbContext, groupUserId, out _groupUser);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            groupUser = new GroupUser();
            BufferMgr.TransferBuffer(_groupUser, groupUser);

            return (new questStatus(Severity.Success));
        }
        public questStatus Read(GroupId groupId, out GroupUserList groupUserList)
        {
            // Initialize
            questStatus status = null;
            groupUserList = null;


            // Get group
            Group group = null;
            status = _dbGroupsMgr.Read(groupId, out group);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get group users
            using (FMSEntities dbContext = new FMSEntities())
            {
                List<Quest.Services.Dbio.FMS.GroupUsers> _groupUserList = null;
                status = read(dbContext, groupId, out _groupUserList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                groupUserList = new GroupUserList();
                groupUserList.Group = group;
                foreach (Quest.Services.Dbio.FMS.GroupUsers _groupUser in _groupUserList)
                {
                    // Get user
                    UserId userId = new UserId(_groupUser.UserId);
                    User user = null;
                    status = _dbUsersMgr.Read(userId, out user);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (status);
                    }
                    groupUserList.UserList.Add(user);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, GroupId groupId, out GroupUserList groupUserList)
        {
            // Initialize
            questStatus status = null;
            groupUserList = null;


            // Get group
            Group group = null;
            status = _dbGroupsMgr.Read(groupId, out group);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get group users
            List<Quest.Services.Dbio.FMS.GroupUsers> _groupUserList = null;
            status = read((FMSEntities)trans.DbContext, groupId, out _groupUserList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            groupUserList = new GroupUserList();
            groupUserList.Group = group;
            foreach (Quest.Services.Dbio.FMS.GroupUsers _groupUser in _groupUserList)
            {
                // Get user
                UserId userId = new UserId(_groupUser.UserId);
                User user = null;
                status = _dbUsersMgr.Read(userId, out user);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                groupUserList.UserList.Add(user);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(UserId userId, out UserGroupList userGroupList)
        {
            // Initialize
            questStatus status = null;
            userGroupList = null;


            // Get user
            User user = null;
            status = _dbUsersMgr.Read(userId, out user);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get user groups
            using (FMSEntities dbContext = new FMSEntities())
            {
                List<Quest.Services.Dbio.FMS.GroupUsers> _groupUserList = null;
                status = read(dbContext, userId, out _groupUserList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                userGroupList = new UserGroupList();
                userGroupList.User = user;
                foreach (Quest.Services.Dbio.FMS.GroupUsers _groupUser in _groupUserList)
                {
                    // Get group
                    GroupId groupId = new GroupId(_groupUser.GroupId);
                    Group group = null;
                    status = _dbGroupsMgr.Read(groupId, out group);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (status);
                    }
                    userGroupList.GroupList.Add(group);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, UserId userId, out UserGroupList userGroupList)
        {
            // Initialize
            questStatus status = null;
            userGroupList = null;


            // Get user
            User user = null;
            status = _dbUsersMgr.Read(userId, out user);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get user groups
            List<Quest.Services.Dbio.FMS.GroupUsers> _groupUserList = null;
            status = read((FMSEntities)trans.DbContext, userId, out _groupUserList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            userGroupList = new UserGroupList();
            userGroupList.User = user;
            foreach (Quest.Services.Dbio.FMS.GroupUsers _groupUser in _groupUserList)
            {
                // Get group
                GroupId groupId = new GroupId(_groupUser.UserId);
                Group group = null;
                status = _dbGroupsMgr.Read(groupId, out group);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                userGroupList.GroupList.Add(group);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(GroupUser groupUser)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (FMSEntities dbContext = new FMSEntities())
            {
                status = update(dbContext, groupUser);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, GroupUser groupUser)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((FMSEntities)trans.DbContext, groupUser);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(GroupUserId groupUserId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (FMSEntities dbContext = new FMSEntities())
            {
                status = delete(dbContext, groupUserId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, GroupUserId groupUserId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((FMSEntities)trans.DbContext, groupUserId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(GroupId groupId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (FMSEntities dbContext = new FMSEntities())
            {
                status = delete(dbContext, groupId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, GroupId groupId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((FMSEntities)trans.DbContext, groupId);
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
        public questStatus List(QueryOptions queryOptions, out List<GroupUser> groupUserList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            groupUserList = null;
            queryResponse = null;


            using (FMSEntities dbContext = new FMSEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.FMS.GroupUsers).GetProperties().ToArray();
                        List<Quest.Services.Dbio.FMS.GroupUsers> _groupUsersList = dbContext.GroupUsers.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_groupUsersList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        groupUserList = new List<GroupUser>();
                        foreach (Quest.Services.Dbio.FMS.GroupUsers _groupUser in _groupUsersList)
                        {
                            GroupUser groupUser = new GroupUser();
                            BufferMgr.TransferBuffer(_groupUser, groupUser);
                            groupUserList.Add(groupUser);
                        }
                        status = BuildQueryResponse(_groupUsersList.Count, queryOptions, out queryResponse);
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
                _dbGroupsMgr = new DbGroupsMgr(this.UserSession);
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


        #region GroupUsers
        /*----------------------------------------------------------------------------------------------------------------------------------
         * GroupUsers
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(FMSEntities dbContext, GroupUser groupUser, out GroupUserId groupUserId)
        {
            // Initialize
            groupUserId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.FMS.GroupUsers _groupUser = new Quest.Services.Dbio.FMS.GroupUsers();
                _groupUser.GroupId = groupUser.Group.Id;
                _groupUser.UserId = groupUser.User.Id;
                _groupUser.Created = DateTime.Now;
                dbContext.GroupUsers.Add(_groupUser);
                dbContext.SaveChanges();
                if (_groupUser.Id == 0)
                {
                    return (new questStatus(Severity.Error, "GroupUser not created"));
                }
                groupUserId = new GroupUserId(_groupUser.Id);
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus read(FMSEntities dbContext, GroupUserId groupUserId, out Quest.Services.Dbio.FMS.GroupUsers groupUser)
        {
            // Initialize
            groupUser = null;


            try
            {
                groupUser = dbContext.GroupUsers.Where(r => r.Id == groupUserId.Id).SingleOrDefault();
                if (groupUser == null)
                {
                    return (new questStatus(Severity.Warning, String.Format("WARNING: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", groupUserId.Id))));
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
        private questStatus read(FMSEntities dbContext, GroupId groupId, out List<Quest.Services.Dbio.FMS.GroupUsers> groupUserList)
        {
            // Initialize
            groupUserList = null;


            try
            {
                groupUserList = dbContext.GroupUsers.Where(r => r.GroupId == groupId.Id).ToList();
                if (groupUserList == null)
                {
                    return (new questStatus(Severity.Warning, String.Format("WARNING: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("GroupId {0} not found", groupId.Id))));
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
        private questStatus read(FMSEntities dbContext, UserId userId, out List<Quest.Services.Dbio.FMS.GroupUsers> groupUserList)
        {
            // Initialize
            groupUserList = null;


            try
            {
                groupUserList = dbContext.GroupUsers.Where(r => r.UserId == userId.Id).ToList();
                if (groupUserList == null)
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
        private questStatus update(FMSEntities dbContext, GroupUser groupUser)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                GroupUserId groupUserId = new GroupUserId(groupUser.Id);
                Quest.Services.Dbio.FMS.GroupUsers _groupUser = null;
                status = read(dbContext, groupUserId, out _groupUser);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(groupUser, _groupUser);
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
        private questStatus delete(FMSEntities dbContext, GroupUserId groupUserId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                Quest.Services.Dbio.FMS.GroupUsers _groupUser = null;
                status = read(dbContext, groupUserId, out _groupUser);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.GroupUsers.Remove(_groupUser);
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
        private questStatus delete(FMSEntities dbContext, GroupId groupId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                List<Quest.Services.Dbio.FMS.GroupUsers> groupUserList = null;
                status = read(dbContext, groupId, out groupUserList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the records.
                foreach (Quest.Services.Dbio.FMS.GroupUsers groupUser in groupUserList)
                {
                    dbContext.GroupUsers.Remove(groupUser);
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
        private questStatus delete(FMSEntities dbContext, UserId userId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                List<Quest.Services.Dbio.FMS.GroupUsers> groupUserList = null;
                status = read(dbContext, userId, out groupUserList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the records.
                foreach (Quest.Services.Dbio.FMS.GroupUsers groupUser in groupUserList)
                {
                    dbContext.GroupUsers.Remove(groupUser);
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
