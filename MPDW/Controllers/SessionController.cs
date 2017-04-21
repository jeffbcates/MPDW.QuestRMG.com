using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.MPDW.Models;
using Quest.MPDW.Modelers;
using Quest.MPDW.Services.Data;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MPDW.Controllers
{
    public class SessionController : BaseController
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        #endregion


        #region GET Methods
        /*==================================================================================================================================
         * GET Methods
         *=================================================================================================================================*/
        [AllowAnonymous]
        public ActionResult Index()
        {
            questStatus status = null;
            UserMessageModeler userMessageModeler = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                userMessageModeler = new UserMessageModeler(status);
                return View("Index", new { Error = userMessageModeler });
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize();
            if (!questStatusDef.IsSuccess(status))
            {
                userMessageModeler = new UserMessageModeler(status);
                return View("Index", new { Error = userMessageModeler });
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            LoginResponseViewModel loginResponseViewModel = null;
            if (TempData["LoginResponseViewModel"] != null)
            {
                loginResponseViewModel = (LoginResponseViewModel)TempData["LoginResponseViewModel"];
            }
            else
            {
                loginResponseViewModel = new LoginResponseViewModel();
            }
            return View(loginResponseViewModel);
        }
        [AllowAnonymous]
        public ActionResult Register()
        {
            questStatus status = null;
            UserMessageModeler userMessageModeler = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                userMessageModeler = new UserMessageModeler(status);
                return View("Index", new { Error = userMessageModeler });
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize();
            if (!questStatusDef.IsSuccess(status))
            {
                userMessageModeler = new UserMessageModeler(status);
                return View("Index", new { Error = userMessageModeler });
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return view
             *---------------------------------------------------------------------------------------------------------------------------------*/
            return RedirectToAction("Index", "Type", new { area = "Registration" });
        }
        #endregion


        #region POST Methods
        /*==================================================================================================================================
         * POST Methods
         *=================================================================================================================================*/
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginRequestViewModel loginRequestViewModel)
        {
            // Initialize
            questStatus status = null;
            LoginResponseViewModel loginResponseViewModel = null;


            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                loginResponseViewModel = new LoginResponseViewModel();
                loginResponseViewModel.UserMessages[0] = new UserMessageModeler(status).UserMessage;
                status.Message = null;
                loginResponseViewModel.questStatus = status;
                TempData["LoginResponseViewModel"] = loginResponseViewModel;
                return RedirectToAction("Index");
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize();
            if (!questStatusDef.IsSuccess(status))
            {
                loginResponseViewModel = new LoginResponseViewModel();
                loginResponseViewModel.UserMessages[0] = new UserMessageModeler(status).UserMessage;
                status.Message = null;
                loginResponseViewModel.questStatus = status;
                TempData["LoginResponseViewModel"] = loginResponseViewModel;
                return RedirectToAction("Index");
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Login
             *---------------------------------------------------------------------------------------------------------------------------------*/
            UserSessionId userSessionId = null;
            AccountModeler accountModeler = new AccountModeler(this.Request, this.UserSession);
            status = accountModeler.Login(Request, loginRequestViewModel, out userSessionId);
            if (!questStatusDef.IsSuccess(status))
            {
                loginResponseViewModel = new LoginResponseViewModel();
                ////loginResponseViewModel.UserMessages.Add(new UserMessageModeler(new questStatus(Severity.Error, "Invalid username or password")).UserMessage);
                loginResponseViewModel.UserMessages.Add(new UserMessageModeler(new questStatus(status.Severity, status.Message)).UserMessage);
                status.Message = null;
                loginResponseViewModel.questStatus = status;
                TempData["LoginResponseViewModel"] = loginResponseViewModel;
                return RedirectToAction("Index");
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Direct user to home page.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            return (RedirectToAction("Index", "Home", new { _ctx = userSessionId.Id }));
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Authenticate(LoginRequestViewModel loginRequestViewModel)
        {
            // Initialize
            questStatus status = null;
            UserMessageModeler userMessageModeler = null;


            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (!questStatusDef.IsSuccess(status))
            {
                userMessageModeler = new UserMessageModeler(status);
                return Json(userMessageModeler, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize();
            if (!questStatusDef.IsSuccess(status))
            {
                userMessageModeler = new UserMessageModeler(status);
                return Json(userMessageModeler, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Login
             *---------------------------------------------------------------------------------------------------------------------------------*/
            UserSessionId userSessionId = null;
            AccountModeler accountModeler = new AccountModeler(this.Request, this.UserSession);
            status = accountModeler.Login(Request, loginRequestViewModel, out userSessionId);
            if (!questStatusDef.IsSuccess(status))
            {
                userMessageModeler = new UserMessageModeler(status);
                return Json(userMessageModeler, JsonRequestBehavior.AllowGet);
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Return result.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            BaseUserSessionViewModel baseUserSessionViewModel = new BaseUserSessionViewModel();
            baseUserSessionViewModel._ctx = userSessionId.Id;
            baseUserSessionViewModel.questStatus = new questStatus(Severity.Success, "Successfully authenticated");
            return Json(baseUserSessionViewModel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Logout(LogoutRequestViewModel logoutRequestViewModel)
        {
            // Initialize
            questStatus status = null;

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Log Operation
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = LogOperation();
            if (! questStatusDef.IsSuccess(status))
            {
                return RedirectToAction("Index", "Home", new { _ctx = logoutRequestViewModel.ctx });
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Authorize
             *---------------------------------------------------------------------------------------------------------------------------------*/
            status = Authorize();
            if (!questStatusDef.IsSuccess(status))
            {
                return RedirectToAction("Index", "Home", new { _ctx = logoutRequestViewModel.ctx });
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Logout
             *---------------------------------------------------------------------------------------------------------------------------------*/
            AccountModeler accountModeler = new AccountModeler(this.Request, this.UserSession);
            status = accountModeler.Logout(logoutRequestViewModel);
            if (!questStatusDef.IsSuccess(status))
            {
                return RedirectToAction("Index", "Home", new { _ctx = logoutRequestViewModel.ctx });
            }

            /*----------------------------------------------------------------------------------------------------------------------------------
             * Direct user to login page.
             *---------------------------------------------------------------------------------------------------------------------------------*/
            return RedirectToAction("Index");
        }
        #endregion


        #region Private Methods
        /*==================================================================================================================================
         * Private Methods
         *=================================================================================================================================*/

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        ////internal class ExternalLoginResult : ActionResult
        ////{
        ////    public ExternalLoginResult(string provider, string returnUrl)
        ////    {
        ////        Provider = provider;
        ////        ReturnUrl = returnUrl;
        ////    }

        ////    public string Provider { get; private set; }
        ////    public string ReturnUrl { get; private set; }

        ////    public override void ExecuteResult(ControllerContext context)
        ////    {
        ////        OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
        ////    }
        ////}

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion

        #endregion
    }
}
