using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.MPDW.Services.Data;
using Quest.MPDW.Models;
using Quest.MPDW.Modelers;
using Quest.MPDW.Services.Business.Accounts;


namespace Quest.MPDW.Modelers
{
    public class AccountModeler : BaseModeler
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
        public AccountModeler(HttpRequestBase httpRequestBase, UserSession userSession)
            : base(httpRequestBase, userSession)
        {

        }
        #endregion


        #region Public Methods
        /*==================================================================================================================================
        * Public Methods
        *=================================================================================================================================*/
        public questStatus Login(HttpRequestBase request, LoginRequestViewModel loginRequestViewModel, out UserSessionId userSessionId)
        {
            // Initialize
            questStatus status = null;
            userSessionId = null;


            // Transfer model
            UserSession userSession = null;
            LoginRequest loginRequest = new LoginRequest();
            BufferMgr.TransferBuffer(loginRequestViewModel, loginRequest);
            string ipAddress = null;
            status = GetIPAddress(request, out ipAddress);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            loginRequest.IPAddress = ipAddress;
            loginRequest.UserAgent = request.UserAgent;


            // Perform login
            AccountMgr accountMgr = new AccountMgr(this.UserSession);
            status = accountMgr.Login(loginRequest, out userSession);
            if (!questStatusDef.IsSuccess(status))
            {
                return(status);
            }

            // Return user session Id
            userSessionId = new UserSessionId(userSession.Id);

            return (new questStatus(Severity.Success));
        }
        public questStatus Logout(LogoutRequestViewModel logoutRequestViewModel)
        {
            // Initialize
            questStatus status = null;


            // Perform logout
            UserSessionId userSessionId = new UserSessionId(logoutRequestViewModel.ctx);
            AccountMgr accountMgr = new AccountMgr(this.UserSession);
            status = accountMgr.Logout(userSessionId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }

        #region Options
        //----------------------------------------------------------------------------------------------------------------------------------
        // Options
        //----------------------------------------------------------------------------------------------------------------------------------
        #endregion

        #endregion


        #region Private Methods
        /*==================================================================================================================================
        * Private Methods
        *=================================================================================================================================*/
        public questStatus initialize()
        {
            return (new questStatus(Severity.Success));
        }
        #endregion
    }

}