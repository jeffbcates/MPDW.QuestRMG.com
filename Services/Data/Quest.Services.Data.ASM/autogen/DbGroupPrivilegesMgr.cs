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
using Quest.MPDW.Services.Data;
using Quest.Services.Dbio.FMS;


namespace Quest.MPDW.Services.Data.Accounts
{
    public class DbGroupPrivilegesMgr : DbMgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbPrivilegesMgr _dbPrivilegesMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public DbGroupPrivilegesMgr(UserSession userSession)
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
        public questStatus Create(GroupPrivilege groupPrivilege, out GroupPrivilegeId groupPrivilegeId)
        {
            // Initialize
            questStatus status = null;
            groupPrivilegeId = null;


            // Data rules.
            groupPrivilege.Created = DateTime.Now;


            // Create the groupPrivilege
            using (FMSEntities dbContext = new FMSEntities())
            {
                status = create(dbContext, groupPrivilege, out groupPrivilegeId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, GroupPrivilege groupPrivilege, out GroupPrivilegeId groupPrivilegeId)
        {
            // Initialize
            questStatus status = null;
            groupPrivilegeId = null;


            // Data rules.
            groupPrivilege.Created = DateTime.Now;


            // Create the groupPrivilege in this transaction.
            status = create((FMSEntities)trans.DbContext, groupPrivilege, out groupPrivilegeId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }

        public questStatus Read(GroupPrivilegeId groupPrivilegeId, out GroupPrivilege groupPrivilege)
        {
            // Initialize
            questStatus status = null;
            groupPrivilege = null;


            // Perform read
            using (FMSEntities dbContext = new FMSEntities())
            {
                Quest.Services.Dbio.FMS.GroupPrivileges _groupPrivilege = null;
                status = read(dbContext, groupPrivilegeId, out _groupPrivilege);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                groupPrivilege = new GroupPrivilege();
                BufferMgr.TransferBuffer(_groupPrivilege, groupPrivilege);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(GroupId groupId, out List<Privilege> privilegeList)
        {
            // Initialize
            questStatus status = null;
            privilegeList = null;


            // Perform read
            using (FMSEntities dbContext = new FMSEntities())
            {
                List<Quest.Services.Dbio.FMS.GroupPrivileges> _groupPrivilegeList = null;
                status = read(dbContext, groupId, out _groupPrivilegeList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                privilegeList = new List<Privilege>();
                foreach (Quest.Services.Dbio.FMS.GroupPrivileges _groupPrivilege in _groupPrivilegeList)
                {
                    PrivilegeId privilegeId = new PrivilegeId(_groupPrivilege.PrivilegeId);
                    Privilege privilege = null;
                    status = _dbPrivilegesMgr.Read(privilegeId, out privilege);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (status);
                    }
                    privilegeList.Add(privilege);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(PrivilegeId privilegeId, out List<Privilege> privilegeList)
        {
            // Initialize
            questStatus status = null;
            privilegeList = null;


            // Perform read
            using (FMSEntities dbContext = new FMSEntities())
            {
                List<Quest.Services.Dbio.FMS.GroupPrivileges> _groupPrivilegeList = null;
                status = read(dbContext, privilegeId, out _groupPrivilegeList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                privilegeList = new List<Privilege>();
                foreach (Quest.Services.Dbio.FMS.GroupPrivileges _groupPrivilege in _groupPrivilegeList)
                {
                    PrivilegeId _privilegeId = new PrivilegeId(_groupPrivilege.PrivilegeId);
                    Privilege privilege = null;
                    status = _dbPrivilegesMgr.Read(_privilegeId, out privilege);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (status);
                    }
                    privilegeList.Add(privilege);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, GroupPrivilegeId groupPrivilegeId, out GroupPrivilege groupPrivilege)
        {
            // Initialize
            questStatus status = null;
            groupPrivilege = null;


            // Perform read
            using (FMSEntities dbContext = new FMSEntities())
            {
                Quest.Services.Dbio.FMS.GroupPrivileges _groupPrivilege = null;
                status = read((FMSEntities)trans.DbContext, groupPrivilegeId, out _groupPrivilege);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                groupPrivilege = new GroupPrivilege();
                BufferMgr.TransferBuffer(_groupPrivilege, groupPrivilege);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, GroupId groupId, out List<Privilege> privilegeList)
        {
            // Initialize
            questStatus status = null;
            privilegeList = null;


            // Perform read
            using (FMSEntities dbContext = new FMSEntities())
            {
                List<Quest.Services.Dbio.FMS.GroupPrivileges> _groupPrivilegeList = null;
                status = read((FMSEntities)trans.DbContext, groupId, out _groupPrivilegeList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                privilegeList = new List<Privilege>();
                foreach (Quest.Services.Dbio.FMS.GroupPrivileges _groupPrivilege in _groupPrivilegeList)
                {
                    PrivilegeId privilegeId = new PrivilegeId(_groupPrivilege.PrivilegeId);
                    Privilege privilege = null;
                    status = _dbPrivilegesMgr.Read(privilegeId, out privilege);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (status);
                    }
                    privilegeList.Add(privilege);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, PrivilegeId privilegeId, out List<Privilege> privilegeList)
        {
            // Initialize
            questStatus status = null;
            privilegeList = null;


            // Perform read
            using (FMSEntities dbContext = new FMSEntities())
            {
                List<Quest.Services.Dbio.FMS.GroupPrivileges> _groupPrivilegeList = null;
                status = read((FMSEntities)trans.DbContext, privilegeId, out _groupPrivilegeList);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                privilegeList = new List<Privilege>();
                foreach (Quest.Services.Dbio.FMS.GroupPrivileges _groupPrivilege in _groupPrivilegeList)
                {
                    PrivilegeId _privilegeId = new PrivilegeId(_groupPrivilege.PrivilegeId);
                    Privilege privilege = null;
                    status = _dbPrivilegesMgr.Read(_privilegeId, out privilege);
                    if (!questStatusDef.IsSuccess(status))
                    {
                        return (status);
                    }
                    privilegeList.Add(privilege);
                }
            }
            return (new questStatus(Severity.Success));
        }

        public questStatus Update(GroupPrivilege groupPrivilege)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (FMSEntities dbContext = new FMSEntities())
            {
                status = update(dbContext, groupPrivilege);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, GroupPrivilege groupPrivilege)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((FMSEntities)trans.DbContext, groupPrivilege);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }

        public questStatus Delete(GroupPrivilegeId groupPrivilegeId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete.
            using (FMSEntities dbContext = new FMSEntities())
            {
                status = delete(dbContext, groupPrivilegeId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, GroupPrivilegeId groupPrivilegeId)
        {
            // Initialize
            questStatus status = null;


            // Perform delete in this transaction.
            status = delete((FMSEntities)trans.DbContext, groupPrivilegeId);
            if (!questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }

        public questStatus List(QueryOptions queryOptions, out List<GroupPrivilege> groupPrivilegeList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            groupPrivilegeList = null;
            queryResponse = null;


            using (FMSEntities dbContext = new FMSEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.FMS.GroupPrivileges).GetProperties().ToArray();
                        List<Quest.Services.Dbio.FMS.GroupPrivileges> _groupPrivilegesList = dbContext.GroupPrivileges.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_groupPrivilegesList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        groupPrivilegeList = new List<GroupPrivilege>();
                        foreach (Quest.Services.Dbio.FMS.GroupPrivileges _groupPrivilege in _groupPrivilegesList)
                        {
                            GroupPrivilege groupPrivilege = new GroupPrivilege();
                            BufferMgr.TransferBuffer(_groupPrivilege, groupPrivilege);
                            groupPrivilegeList.Add(groupPrivilege);
                        }
                        status = BuildQueryResponse(_groupPrivilegesList.Count, queryOptions, out queryResponse);
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
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                throw new System.Exception(status.Message, ex);
            }
            return (new questStatus(Severity.Success));
        }


        #region GroupPrivileges
        /*----------------------------------------------------------------------------------------------------------------------------------
         * GroupPrivileges
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(FMSEntities dbContext, GroupPrivilege groupPrivilege, out GroupPrivilegeId groupPrivilegeId)
        {
            // Initialize
            groupPrivilegeId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.FMS.GroupPrivileges _groupPrivilege = new Quest.Services.Dbio.FMS.GroupPrivileges();
                BufferMgr.TransferBuffer(groupPrivilege, _groupPrivilege);
                dbContext.GroupPrivileges.Add(_groupPrivilege);
                dbContext.SaveChanges();
                if (_groupPrivilege.Id == 0)
                {
                    return (new questStatus(Severity.Error, "GroupPrivilege not created"));
                }
                groupPrivilegeId = new GroupPrivilegeId(_groupPrivilege.Id);
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus read(FMSEntities dbContext, GroupPrivilegeId groupPrivilegeId, out Quest.Services.Dbio.FMS.GroupPrivileges groupPrivilege)
        {
            // Initialize
            groupPrivilege = null;


            try
            {
                groupPrivilege = dbContext.GroupPrivileges.Where(r => r.Id == groupPrivilegeId.Id).SingleOrDefault();
                if (groupPrivilege == null)
                {
                    return (new questStatus(Severity.Warning, String.Format("WARNING: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", groupPrivilegeId.Id))));
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
        private questStatus read(FMSEntities dbContext, GroupId groupId, out List<Quest.Services.Dbio.FMS.GroupPrivileges> groupPrivilegeList)
        {
            // Initialize
            groupPrivilegeList = null;


            try
            {
                groupPrivilegeList = dbContext.GroupPrivileges.Where(r => r.GroupId == groupId.Id).ToList();
                if (groupPrivilegeList == null)
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
        private questStatus read(FMSEntities dbContext, PrivilegeId privilegeId, out List<Quest.Services.Dbio.FMS.GroupPrivileges> groupPrivilegeList)
        {
            // Initialize
            groupPrivilegeList = null;


            try
            {
                groupPrivilegeList = dbContext.GroupPrivileges.Where(r => r.PrivilegeId == privilegeId.Id).ToList();
                if (groupPrivilegeList == null)
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
        private questStatus update(FMSEntities dbContext, GroupPrivilege groupPrivilege)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                GroupPrivilegeId groupPrivilegeId = new GroupPrivilegeId(groupPrivilege.Id);
                Quest.Services.Dbio.FMS.GroupPrivileges _groupPrivilege = null;
                status = read(dbContext, groupPrivilegeId, out _groupPrivilege);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(groupPrivilege, _groupPrivilege);
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
        private questStatus delete(FMSEntities dbContext, GroupPrivilegeId groupPrivilegeId)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                Quest.Services.Dbio.FMS.GroupPrivileges _groupPrivilege = null;
                status = read(dbContext, groupPrivilegeId, out _groupPrivilege);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.GroupPrivileges.Remove(_groupPrivilege);
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
