using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Util.Encryption;
using Quest.Functional.ASM;
using Quest.MPDW.Services.Business;
using Quest.MPDW.Services.Data;
using Quest.MPDW.Services.Data.Accounts;


namespace Quest.MPDW.Services.Business.Accounts
{
    public class AccountMgr : MgrSessionBased
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private DbUsersMgr _dbUsersMgr = null;
        private DbAccountsMgr _dbAccountsMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public AccountMgr(UserSession userSession)
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
        public questStatus Login(LoginRequest loginRequest, out UserSession userSession)
        {
            // Initialize
            userSession = null;
            questStatus status = null;

            /*
             * Verify user account requirements to login
             */
            // Read user
            User user = null;
            status = _dbUsersMgr.Read(loginRequest.Username, out user);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // User must be enabled and active.
            if (!user.bEnabled)
            {
                return (new questStatus(Severity.Error, "User is not enabled"));
            }
            if (!user.bActive)
            {
                return (new questStatus(Severity.Error, "User is not active"));
            }

            // Verify password.
            AESEncryption aesEncryption = new AESEncryption();
            string encryptedLoginPassword = aesEncryption.Encrypt(loginRequest.Password);
            if (string.Compare(encryptedLoginPassword, user.Password) != 0)
            {
                return (new questStatus(Severity.Error, "Invalid user credentials"));
            }


            // Create user session
            status = _dbAccountsMgr.Login(loginRequest, user, out userSession);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Logout(UserSessionId userSessionId)
        {
            questStatus status = null;
            UserSession userSession = null;

            // Get the user session.
            UserSessionMgr userSessionMgr = new UserSessionMgr();
            status = userSessionMgr.Read(userSessionId, out userSession);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            } 

            // Set termination date to now.
            userSession.Terminated = DateTime.Now;


            // Update the user session.
            status = userSessionMgr.Update(userSession);
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
                _dbUsersMgr = new DbUsersMgr(this.UserSession);
                _dbAccountsMgr = new DbAccountsMgr(this.UserSession);
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
