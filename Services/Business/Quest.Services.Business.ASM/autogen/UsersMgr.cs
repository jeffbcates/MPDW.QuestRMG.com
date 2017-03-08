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
    public class UsersMgr : Mgr
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbUsersMgr _dbUsersMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public UsersMgr()
            : base()
        {
            initialize();
        }
        public UsersMgr(UserSession userSession)
            : base()
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
        public questStatus Create(User user, out UserId userId)
        {
            // Initialize
            userId = null;
            questStatus status = null;


            // If no password, use a blank
            if (string.IsNullOrEmpty(user.Password))
            {
                user.Password = "";
            }

            // Create user
            status = _dbUsersMgr.Create(user, out userId);
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read(UserId userId, out User user)
        {
            // Initialize
            user = null;
            questStatus status = null;


            // Read user
            status = _dbUsersMgr.Read(userId, out user);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Update(User user)
        {
            // Initialize
            questStatus status = null;


            // If no password, read user and copy current password.
            if (string.IsNullOrEmpty(user.Password))
            {
                UserId userId = new UserId(user.Id);
                User _user = null;
                status = Read(userId, out _user);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
                user.Password = _user.Password;
            }


            // Update user
            status = _dbUsersMgr.Update(user);
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


            // Delete user
            status = _dbUsersMgr.Delete(userId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus List(QueryOptions queryOptions, out List<User> userList, out QueryResponse queryResponse)
        {
            // Initialize
            questStatus status = null;
            userList = null;


            // List users
            status = _dbUsersMgr.List(queryOptions, out userList, out queryResponse);
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
                _dbUsersMgr = new DbUsersMgr(new UserSession()); // TODO: UNSTUB THIS LATER.
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
