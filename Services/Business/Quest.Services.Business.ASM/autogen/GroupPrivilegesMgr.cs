using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.MPDW.Services.Data;
using Quest.MPDW.Services.Business;
using Quest.MPDW.Services.Data.Accounts;


namespace Quest.MPDW.Services.Business.Accounts
{
    public class GroupPrivilegesMgr : MgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbGroupPrivilegesMgr _dbGroupPrivilegesMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public GroupPrivilegesMgr(UserSession userSession)
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



            // Create groupPrivilege
            status = _dbGroupPrivilegesMgr.Create(groupPrivilege, out groupPrivilegeId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, GroupPrivilege groupPrivilege, out GroupPrivilegeId groupPrivilegeId)
        {
            // Initialize
            questStatus status = null;
            groupPrivilegeId = null;



            // Create groupPrivilege
            status = _dbGroupPrivilegesMgr.Create(trans, groupPrivilege, out groupPrivilegeId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(GroupPrivilegeId groupPrivilegeId, out GroupPrivilege groupPrivilege)
        {
            // Initialize
            groupPrivilege = null;
            questStatus status = null;


            // Read groupPrivilege
            status = _dbGroupPrivilegesMgr.Read(groupPrivilegeId, out groupPrivilege);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, GroupPrivilegeId groupPrivilegeId, out GroupPrivilege groupPrivilege)
        {
            // Initialize
            questStatus status = null;
            groupPrivilege = null;


            // Read groupPrivilege
            status = _dbGroupPrivilegesMgr.Read(trans, groupPrivilegeId, out groupPrivilege);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(GroupId groupId, out GroupPrivilegeList groupPrivilegeList)
        {
            // Initialize
            questStatus status = null;
            groupPrivilegeList = null;


            // Read groupPrivilege
            status = _dbGroupPrivilegesMgr.Read(groupId, out groupPrivilegeList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, GroupId groupId, out GroupPrivilegeList groupPrivilegeList)
        {
            // Initialize
            questStatus status = null;
            groupPrivilegeList = null;


            // Read groupPrivilege
            status = _dbGroupPrivilegesMgr.Read(trans, groupId, out groupPrivilegeList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(PrivilegeId privilegeId, out PrivilegeGroupList privilegeGroupList)
        {
            // Initialize
            questStatus status = null;
            privilegeGroupList = null;


            // Read groupPrivilege
            status = _dbGroupPrivilegesMgr.Read(privilegeId, out privilegeGroupList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, PrivilegeId privilegeId, out PrivilegeGroupList privilegeGroupList)
        {
            // Initialize
            questStatus status = null;
            privilegeGroupList = null;


            // Read groupPrivilege
            status = _dbGroupPrivilegesMgr.Read(trans, privilegeId, out privilegeGroupList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(GroupPrivilege groupPrivilege)
        {
            // Initialize
            questStatus status = null;


            // Update groupPrivilege
            status = _dbGroupPrivilegesMgr.Update(groupPrivilege);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, GroupPrivilege groupPrivilege)
        {
            // Initialize
            questStatus status = null;


            // Update groupPrivilege
            status = _dbGroupPrivilegesMgr.Update(trans, groupPrivilege);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(GroupPrivilegeId groupPrivilegeId)
        {
            // Initialize
            questStatus status = null;


            // Delete groupPrivilege
            status = _dbGroupPrivilegesMgr.Delete(groupPrivilegeId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, GroupPrivilegeId groupPrivilegeId)
        {
            // Initialize
            questStatus status = null;


            // Delete groupPrivilege
            status = _dbGroupPrivilegesMgr.Delete(trans, groupPrivilegeId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(GroupId groupId)
        {
            // Initialize
            questStatus status = null;


            // Delete groupPrivilege
            status = _dbGroupPrivilegesMgr.Delete(groupId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, GroupId groupId)
        {
            // Initialize
            questStatus status = null;


            // Delete groupPrivilege
            status = _dbGroupPrivilegesMgr.Delete(trans, groupId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(PrivilegeId privilegeId)
        {
            // Initialize
            questStatus status = null;


            // Delete groupPrivilege
            status = _dbGroupPrivilegesMgr.Delete(privilegeId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, PrivilegeId privilegeId)
        {
            // Initialize
            questStatus status = null;


            // Delete groupPrivilege
            status = _dbGroupPrivilegesMgr.Delete(trans, privilegeId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<GroupPrivilege> groupPrivilegeList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            groupPrivilegeList = null;


            // List groupPrivileges
            status = _dbGroupPrivilegesMgr.List(queryOptions, out groupPrivilegeList, out queryResponse);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
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
                _dbGroupPrivilegesMgr = new DbGroupPrivilegesMgr(this.UserSession); 
            }
            catch (System.Exception ex)
            {
                status = new questStatus(Severity.Fatal, String.Format("EXCEPTION: {0}.{1}: {2}",
                        this.GetType().ToString(), MethodInfo.GetCurrentMethod().Name, ex.Message));
                throw new System.Exception(status.Message, ex);
            }
            return (new questStatus(Severity.Success));
        }

        #endregion
    }
}
