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
    public class DbGroupsMgr : DbMgrSessionBased
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
        public DbGroupsMgr(UserSession userSession)
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
        public questStatus Create(Group group, out GroupId groupId)
        {
            // Initialize
            questStatus status = null;
            groupId = null;


            // Data rules.
            group.Created = DateTime.Now;


            // Create the group
            using (FMSEntities dbContext = new FMSEntities())
            {
                status = create(dbContext, group, out groupId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Group group, out GroupId groupId)
        {
            // Initialize
            questStatus status = null;
            groupId = null;


            // Data rules.
            group.Created = DateTime.Now;


            // Create the group in this transaction.
            status = create((FMSEntities)trans.DbContext, group, out groupId);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(GroupId groupId, out Group group)
        {
            // Initialize
            questStatus status = null;
            group = null;


            // Perform read
            using (FMSEntities dbContext = new FMSEntities())
            {
                Quest.Services.Dbio.FMS.Groups _group = null;
                status = read(dbContext, groupId, out _group);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                group = new Group();
                BufferMgr.TransferBuffer(_group, group);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(string groupname, out Group group)
        {
            // Initialize
            questStatus status = null;
            group = null;


            // Perform read
            using (FMSEntities dbContext = new FMSEntities())
            {
                Quest.Services.Dbio.FMS.Groups _group = null;
                status = read(dbContext, groupname, out _group);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                group = new Group();
                BufferMgr.TransferBuffer(_group, group);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, GroupId groupId, out Group group)
        {
            // Initialize
            questStatus status = null;
            group = null;


            // Perform read.
            using (FMSEntities dbContext = new FMSEntities())
            {
                Quest.Services.Dbio.FMS.Groups _group = null;
                status = read((FMSEntities)trans.DbContext, groupId, out _group);
                if (! questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                group = new Group();
                BufferMgr.TransferBuffer(_group, group);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, string groupname, out Group group)
        {
            // Initialize
            questStatus status = null;
            group = null;


            // Perform transaction read.
            Quest.Services.Dbio.FMS.Groups _group = null;
            status = read((FMSEntities)trans.DbContext, groupname, out _group);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            group = new Group();
            BufferMgr.TransferBuffer(_group, group);
            
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Group group)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (FMSEntities dbContext = new FMSEntities())
            {
                status = update(dbContext, group);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Group group)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((FMSEntities)trans.DbContext, group);
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
            if (! questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Group> groupList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            groupList = null;
            queryResponse = null;


            using (FMSEntities dbContext = new FMSEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.FMS.Groups).GetProperties().ToArray();
                        int totalRecords = dbContext.Groups.Where(BuildWhereClause(queryOptions, dbProperties)).Count();
                        List<Quest.Services.Dbio.FMS.Groups> _groupsList = dbContext.Groups.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_groupsList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        groupList = new List<Group>();
                        foreach (Quest.Services.Dbio.FMS.Groups _group in _groupsList)
                        {
                            Group group = new Group();
                            BufferMgr.TransferBuffer(_group, group);
                            groupList.Add(group);
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


        #region Groups
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Groups
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(FMSEntities dbContext, Group group, out GroupId groupId)
        {
            // Initialize
            groupId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.FMS.Groups _group = new Quest.Services.Dbio.FMS.Groups();
                BufferMgr.TransferBuffer(group, _group);
                dbContext.Groups.Add(_group);
                dbContext.SaveChanges();
                if (_group.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Group not created"));
                }
                groupId = new GroupId(_group.Id);
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus read(FMSEntities dbContext, GroupId groupId, out Quest.Services.Dbio.FMS.Groups group)
        {
            // Initialize
            group = null;


            try
            {
                group = dbContext.Groups.Where(r => r.Id == groupId.Id).SingleOrDefault();
                if (group == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", groupId.Id))));
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
        private questStatus read(FMSEntities dbContext, string groupname, out Quest.Services.Dbio.FMS.Groups group)
        {
            // Initialize
            group = null;


            try
            {
                group = dbContext.Groups.Where(r => r.Name == groupname).SingleOrDefault();
                if (group == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", groupname))));
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
        private questStatus update(FMSEntities dbContext, Group group)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                GroupId groupId = new GroupId(group.Id);
                Quest.Services.Dbio.FMS.Groups _group = null;
                status = read(dbContext, groupId, out _group);
                if (! questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(group, _group);
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
                Quest.Services.Dbio.FMS.Groups _group = null;
                status = read(dbContext, groupId, out _group);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.Groups.Remove(_group);
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
