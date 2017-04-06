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
    public class UserPrivilegesMgr : MgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbUserPrivilegesMgr _dbUserPrivilegesMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public UserPrivilegesMgr(UserSession userSession)
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



            // Create userPrivilege
            status = _dbUserPrivilegesMgr.Create(userPrivilege, out userPrivilegeId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Create(DbMgrTransaction trans, UserPrivilege userPrivilege, out UserPrivilegeId userPrivilegeId)
        {
            // Initialize
            questStatus status = null;
            userPrivilegeId = null;



            // Create userPrivilege
            status = _dbUserPrivilegesMgr.Create(trans, userPrivilege, out userPrivilegeId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(UserPrivilegeId userPrivilegeId, out UserPrivilege userPrivilege)
        {
            // Initialize
            userPrivilege = null;
            questStatus status = null;


            // Read userPrivilege
            status = _dbUserPrivilegesMgr.Read(userPrivilegeId, out userPrivilege);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, UserPrivilegeId userPrivilegeId, out UserPrivilege userPrivilege)
        {
            // Initialize
            questStatus status = null;
            userPrivilege = null;


            // Read userPrivilege
            status = _dbUserPrivilegesMgr.Read(trans, userPrivilegeId, out userPrivilege);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(UserId userId, out UserPrivilegeList userPrivilegeList)
        {
            // Initialize
            questStatus status = null;
            userPrivilegeList = null;


            // Read userPrivilege
            status = _dbUserPrivilegesMgr.Read(userId, out userPrivilegeList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, UserId userId, out UserPrivilegeList userPrivilegeList)
        {
            // Initialize
            questStatus status = null;
            userPrivilegeList = null;


            // Read userPrivilege
            status = _dbUserPrivilegesMgr.Read(trans, userId, out userPrivilegeList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(PrivilegeId privilegeId, out PrivilegeUserList privilegeUserList)
        {
            // Initialize
            questStatus status = null;
            privilegeUserList = null;


            // Read userPrivilege
            status = _dbUserPrivilegesMgr.Read(privilegeId, out privilegeUserList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(DbMgrTransaction trans, PrivilegeId privilegeId, out PrivilegeUserList privilegeUserList)
        {
            // Initialize
            questStatus status = null;
            privilegeUserList = null;


            // Read userPrivilege
            status = _dbUserPrivilegesMgr.Read(trans, privilegeId, out privilegeUserList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(UserPrivilege userPrivilege)
        {
            // Initialize
            questStatus status = null;


            // Update userPrivilege
            status = _dbUserPrivilegesMgr.Update(userPrivilege);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(DbMgrTransaction trans, UserPrivilege userPrivilege)
        {
            // Initialize
            questStatus status = null;


            // Update userPrivilege
            status = _dbUserPrivilegesMgr.Update(trans, userPrivilege);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(UserPrivilegeId userPrivilegeId)
        {
            // Initialize
            questStatus status = null;


            // Delete userPrivilege
            status = _dbUserPrivilegesMgr.Delete(userPrivilegeId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, UserPrivilegeId userPrivilegeId)
        {
            // Initialize
            questStatus status = null;


            // Delete userPrivilege
            status = _dbUserPrivilegesMgr.Delete(trans, userPrivilegeId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(UserId userId)
        {
            // Initialize
            questStatus status = null;


            // Delete userPrivilege
            status = _dbUserPrivilegesMgr.Delete(userId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(DbMgrTransaction trans, UserId userId)
        {
            // Initialize
            questStatus status = null;


            // Delete userPrivilege
            status = _dbUserPrivilegesMgr.Delete(trans, userId);
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


            // Delete userPrivilege
            status = _dbUserPrivilegesMgr.Delete(privilegeId);
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


            // Delete userPrivilege
            status = _dbUserPrivilegesMgr.Delete(trans, privilegeId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<UserPrivilege> userPrivilegeList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            userPrivilegeList = null;


            // List userPrivileges
            status = _dbUserPrivilegesMgr.List(queryOptions, out userPrivilegeList, out queryResponse);
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
                _dbUserPrivilegesMgr = new DbUserPrivilegesMgr(this.UserSession); 
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
