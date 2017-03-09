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
    public class DbAccountsMgr : DbMgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbUsersMgr _dbUsersMgr = null;
        private DbUserPrivilegesMgr _dbUserPrivilegesMgr = null;
        private DbGroupUsersMgr _dbGroupUsersMgr = null;
        private DbGroupPrivilegesMgr _dbGroupPrivilegesMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public DbAccountsMgr(UserSession userSession)
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
        public questStatus Login(LoginRequest loginRequest, User user, out UserSession userSession)
        {
            // Initialize
            questStatus status = null;
            userSession = null;


            // User must be enabled and active.
            // (Intended to be data rule AND business rule).
            if (!user.bEnabled)
            {
                return (new questStatus(Severity.Error, "User is not enabled"));
            }
            if (!user.bActive)
            {
                return (new questStatus(Severity.Error, "User is not active"));
            }


            // Get user privileges
            UserId userId = new UserId(user.Id);
            List<Privilege> userPrivilegeList = null;
            status = GetUserPrivileges(userId, out userPrivilegeList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Build user session
            UserSession _userSession = new UserSession();
            _userSession.IPAddress = loginRequest.IPAddress;
            _userSession.UserAgent = loginRequest.UserAgent;
            _userSession.LastAction = DateTime.Now;
            _userSession.UserId = user.Id;
            _userSession.User = user;
            _userSession.Privileges = userPrivilegeList;
            _userSession.Created = DateTime.Now;


            // Write the user session
            UserSessionId _userSessionId = null;
            DbUserSessionsMgr dbUserSessionsMgr = new DbUserSessionsMgr(this.UserSession);
            status = dbUserSessionsMgr.Create(_userSession, out _userSessionId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Return the user session
            UserSession __userSession = null;
            status = dbUserSessionsMgr.Read(_userSessionId, out __userSession);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            userSession = __userSession;
            userSession.User = user;
            userSession.Privileges = userPrivilegeList;

            return (new questStatus(Severity.Success));
        }
        public questStatus GetUserGroups(UserId userId, out List<Group> groupList)
        {
            // Initialize
            questStatus status = null;
            groupList = null;


            // Get all user groups.
            UserGroupList userGroupList = null;
            status = _dbGroupUsersMgr.Read(userId, out userGroupList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Return groups
            groupList = userGroupList.GroupList;

            return (new questStatus(Severity.Success));
        }
        public questStatus GetUserPrivileges(UserId userId, out List<Privilege> privilegeList)
        {
            // Initialize
            questStatus status = null;
            privilegeList = null;
            List<Privilege> userPrivilegeList = null;


            // Get all user groups.
            UserGroupList userGroupList = null;
            status = _dbGroupUsersMgr.Read(userId, out userGroupList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get normalized list of all privileges from all groups.
            userPrivilegeList = new List<Privilege>();
            List<Privilege> _privilegeList = null;
            foreach (Group group in userGroupList.GroupList)
            {
                GroupId groupId = new GroupId(group.Id);

                status = _dbGroupPrivilegesMgr.Read(groupId, out _privilegeList);
                if (!questStatusDef.IsSuccessOrWarning(status))
                {
                    return (status);
                }
                foreach (Privilege privilege in _privilegeList)
                {
                    Privilege p = userPrivilegeList.Find(delegate(Privilege _p) { return (_p.Id == privilege.Id); });
                    if (p == null)
                    {
                        userPrivilegeList.Add(privilege);
                    }
                }
            }

            // Get all user privileges and add to session list if not there now.
            status = _dbUserPrivilegesMgr.Read(userId, out _privilegeList);
            if (!questStatusDef.IsSuccessOrWarning(status))
            {
                return (status);
            }
            foreach (Privilege privilege in _privilegeList)
            {
                Privilege p = userPrivilegeList.Find(delegate(Privilege _p) { return (_p.Id == privilege.Id); });
                if (p == null)
                {
                    userPrivilegeList.Add(privilege);
                }
            }

            // Return privileges
            privilegeList = userPrivilegeList;

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
                _dbUsersMgr = new DbUsersMgr(this.UserSession);
                _dbUserPrivilegesMgr = new DbUserPrivilegesMgr(this.UserSession);
                _dbGroupUsersMgr = new DbGroupUsersMgr(this.UserSession);
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
