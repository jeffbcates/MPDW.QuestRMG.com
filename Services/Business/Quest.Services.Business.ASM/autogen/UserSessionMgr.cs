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
    public class UserSessionMgr : MgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbUserSessionsMgr _dbUserSessionsMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public UserSessionMgr(UserSession userSession)
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
            userSessionId = null;
            questStatus status = null;


            // Create userSession
            status = _dbUserSessionsMgr.Create(userSession, out userSessionId);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(UserSessionId userSessionId, out UserSession userSession)
        {
            // Initialize
            userSession = null;
            questStatus status = null;


            // Read userSession
            status = _dbUserSessionsMgr.Read(userSessionId, out userSession);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Get the user
            UserId userId = new UserId(userSession.UserId);
            User user = null;
            UsersMgr usersMgr = new UsersMgr(this.UserSession); // TODO: PLUG UP REAL USER SESSION
            status = usersMgr.Read(userId, out user);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            userSession.User = user;

            // Get user privileges.
            List<Privilege> privilegeList = null;
            DbAccountsMgr dbAccountsMgr = new DbAccountsMgr(new UserSession()); // TODO: UNSTUB 
            status = dbAccountsMgr.GetUserPrivileges(userId, out privilegeList);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            userSession.Privileges = privilegeList;

            return (new questStatus(Severity.Success));
        }
        public questStatus Update(UserSession userSession)
        {
            // Initialize
            questStatus status = null;


            // Update userSession
            status = _dbUserSessionsMgr.Update(userSession);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Delete(UserSessionId userSessionId)
        {
            // Initialize
            questStatus status = null;


            // Delete userSession
            status = _dbUserSessionsMgr.Delete(userSessionId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<UserSession> userSessionList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            userSessionList = null;


            // List userSessions
            status = _dbUserSessionsMgr.List(queryOptions, out userSessionList, out queryResponse);
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
                _dbUserSessionsMgr = new DbUserSessionsMgr(new UserSession()); // TODO: UNSTUB
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
