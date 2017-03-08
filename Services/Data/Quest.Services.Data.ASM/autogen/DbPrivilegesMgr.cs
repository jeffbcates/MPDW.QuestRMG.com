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
    public class DbPrivilegesMgr : DbMgrSessionBased
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
        public DbPrivilegesMgr(UserSession userSession)
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
        public questStatus Create(Privilege privilege, out PrivilegeId privilegeId)
        {
            // Initialize
            questStatus status = null;
            privilegeId = null;


            // Data rules.
            privilege.Created = DateTime.Now;


            // Create the privilege
            using (FMSEntities dbContext = new FMSEntities())
            {
                status = create(dbContext, privilege, out privilegeId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, Privilege privilege, out PrivilegeId privilegeId)
        {
            // Initialize
            questStatus status = null;
            privilegeId = null;


            // Data rules.
            privilege.Created = DateTime.Now;


            // Create the privilege in this transaction.
            status = create((FMSEntities)trans.DbContext, privilege, out privilegeId);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(PrivilegeId privilegeId, out Privilege privilege)
        {
            // Initialize
            questStatus status = null;
            privilege = null;


            // Perform read
            using (FMSEntities dbContext = new FMSEntities())
            {
                Quest.Services.Dbio.FMS.Privileges _privilege = null;
                status = read(dbContext, privilegeId, out _privilege);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                privilege = new Privilege();
                BufferMgr.TransferBuffer(_privilege, privilege);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(string privilegename, out Privilege privilege)
        {
            // Initialize
            questStatus status = null;
            privilege = null;


            // Perform read
            using (FMSEntities dbContext = new FMSEntities())
            {
                Quest.Services.Dbio.FMS.Privileges _privilege = null;
                status = read(dbContext, privilegename, out _privilege);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                privilege = new Privilege();
                BufferMgr.TransferBuffer(_privilege, privilege);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, PrivilegeId privilegeId, out Privilege privilege)
        {
            // Initialize
            questStatus status = null;
            privilege = null;


            // Perform read.
            using (FMSEntities dbContext = new FMSEntities())
            {
                Quest.Services.Dbio.FMS.Privileges _privilege = null;
                status = read((FMSEntities)trans.DbContext, privilegeId, out _privilege);
                if (! questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                privilege = new Privilege();
                BufferMgr.TransferBuffer(_privilege, privilege);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, string privilegename, out Privilege privilege)
        {
            // Initialize
            questStatus status = null;
            privilege = null;


            // Perform transaction read.
            Quest.Services.Dbio.FMS.Privileges _privilege = null;
            status = read((FMSEntities)trans.DbContext, privilegename, out _privilege);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            privilege = new Privilege();
            BufferMgr.TransferBuffer(_privilege, privilege);
            
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(Privilege privilege)
        {
            // Initialize
            questStatus status = null;


            // Perform update.
            using (FMSEntities dbContext = new FMSEntities())
            {
                status = update(dbContext, privilege);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, Privilege privilege)
        {
            // Initialize
            questStatus status = null;
            bool bCreateTransaction = trans == null;


            // Perform update in this transaction.
            status = update((FMSEntities)trans.DbContext, privilege);
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
            if (! questStatusDef.IsSuccess(status))
            {
                RollbackTransaction(trans);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<Privilege> privilegeList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            privilegeList = null;
            queryResponse = null;


            using (FMSEntities dbContext = new FMSEntities())
            {
                using (DbContextTransaction tx = dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        PropertyInfo[] dbProperties = typeof(Quest.Services.Dbio.FMS.Privileges).GetProperties().ToArray();
                        int totalRecords = dbContext.Privileges.Where(BuildWhereClause(queryOptions, dbProperties)).Count();
                        List<Quest.Services.Dbio.FMS.Privileges> _privilegesList = dbContext.Privileges.Where(BuildWhereClause(queryOptions, dbProperties))
                                .OrderBy(BuildSortString(queryOptions.SortColumns))
                                .Skip(queryOptions.Paging.PageSize * (queryOptions.Paging.PageNumber - 1))
                                .Take(queryOptions.Paging.PageSize).ToList();
                        if (_privilegesList == null)
                        {
                            return (new questStatus(Severity.Warning));
                        }
                        privilegeList = new List<Privilege>();
                        foreach (Quest.Services.Dbio.FMS.Privileges _privilege in _privilegesList)
                        {
                            Privilege privilege = new Privilege();
                            BufferMgr.TransferBuffer(_privilege, privilege);
                            privilegeList.Add(privilege);
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


        #region Privileges
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Privileges
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus create(FMSEntities dbContext, Privilege privilege, out PrivilegeId privilegeId)
        {
            // Initialize
            privilegeId = null;


            // Perform create
            try
            {
                Quest.Services.Dbio.FMS.Privileges _privilege = new Quest.Services.Dbio.FMS.Privileges();
                BufferMgr.TransferBuffer(privilege, _privilege);
                dbContext.Privileges.Add(_privilege);
                dbContext.SaveChanges();
                if (_privilege.Id == 0)
                {
                    return (new questStatus(Severity.Error, "Privilege not created"));
                }
                privilegeId = new PrivilegeId(_privilege.Id);
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        ex.InnerException != null ? ex.InnerException.Message : ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus read(FMSEntities dbContext, PrivilegeId privilegeId, out Quest.Services.Dbio.FMS.Privileges privilege)
        {
            // Initialize
            privilege = null;


            try
            {
                privilege = dbContext.Privileges.Where(r => r.Id == privilegeId.Id).SingleOrDefault();
                if (privilege == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", privilegeId.Id))));
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
        private questStatus read(FMSEntities dbContext, string privilegename, out Quest.Services.Dbio.FMS.Privileges privilege)
        {
            // Initialize
            privilege = null;


            try
            {
                privilege = dbContext.Privileges.Where(r => r.Name == privilegename).SingleOrDefault();
                if (privilege == null)
                {
                    return (new questStatus(Severity.Error, String.Format("ERROR: {0}.{1}: {2}",
                            this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                            String.Format("Id {0} not found", privilegename))));
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
        private questStatus update(FMSEntities dbContext, Privilege privilege)
        {
            // Initialize 
            questStatus status = null;


            try
            {
                // Read the record.
                PrivilegeId privilegeId = new PrivilegeId(privilege.Id);
                Quest.Services.Dbio.FMS.Privileges _privilege = null;
                status = read(dbContext, privilegeId, out _privilege);
                if (! questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Update the record.
                BufferMgr.TransferBuffer(privilege, _privilege);
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
                Quest.Services.Dbio.FMS.Privileges _privilege = null;
                status = read(dbContext, privilegeId, out _privilege);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }

                // Delete the record.
                dbContext.Privileges.Remove(_privilege);
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
