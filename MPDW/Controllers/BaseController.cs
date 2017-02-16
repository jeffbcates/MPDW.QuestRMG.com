using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.MPDW.Services.Data;
using Quest.MPDW.Services.Business.Accounts;
using Quest.MPDW.Models;
using Quest.MPDW.Modelers;


namespace Quest.MPDW.Controllers
{
    public class BaseController : Controller
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private UserSession _userSession = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        #endregion


        #region Properties
        /*==================================================================================================================================
         * Properties
         *=================================================================================================================================*/
        public UserSession UserSession
        {
            get
            {
                return (this._userSession);
            }
        }
        #endregion


        #region Public Methods
        /*==================================================================================================================================
         * Public Methods
         *=================================================================================================================================*/
        protected override void HandleUnknownAction(string actionName)
        {
            // NOTE: THIS SHOULD DISPLAY NON-USER SESSION-BASED ERROR INFORMATION

            Response.RedirectPermanent("~/Error");
        }
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }


        #region Request Processing
        /*==================================================================================================================================
         * Request Processing
         *=================================================================================================================================*/
        public RouteValueDictionary PropagateQueryString(HttpRequestBase httpRequestBase)
        {
            NameValueCollection nvcQueryString = HttpUtility.ParseQueryString(Request.Url.Query);
            Dictionary<string, object> dictQueryString = nvcQueryString.AllKeys.ToDictionary(k => k, k => (object)nvcQueryString[k]);
            return (new RouteValueDictionary(dictQueryString));
        }
        #endregion


        #region Options
        /*==================================================================================================================================
         * Options
         *=================================================================================================================================*/

        #region Geo Options
        //----------------------------------------------------------------------------------------------------------------------------------
        // Geo Options
        //----------------------------------------------------------------------------------------------------------------------------------
        #endregion
        
        #endregion


        #region Logging
        /*==================================================================================================================================
         * Logging
         *=================================================================================================================================*/
        public questStatus LogOperation()
        {
            return (new questStatus(Severity.Success));
        }
        #endregion


        #region Authorization
        /*==================================================================================================================================
         * Authorization
         *=================================================================================================================================*/
        public questStatus Authorize()
        {
            // NOTE: THIS WOULD BE A NON-USER SESSION-BASED AUTHORIZATION, E.G. IP ADDRESS, USER-AGENT, OTHER.

            return (new questStatus(Severity.Success));
        }
        public questStatus Authorize(object userSessionViewModel)
        {
            BaseUserSessionViewModel baseUserSessionViewModel = (BaseUserSessionViewModel)userSessionViewModel;

            questStatus status = null;
            status = Authorize(baseUserSessionViewModel._ctx);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            baseUserSessionViewModel.SetUserSession(this._userSession);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Authorize(int ctx)
        {
            questStatus status = null;
            UserSessionId userSessionId = new UserSessionId(ctx);
            UserSession userSession = null;
            status = ValidateUserSession(userSessionId, out userSession);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            this._userSession = userSession;
            return (new questStatus(Severity.Success));
        }
        public questStatus ValidateUserSession(UserSessionId userSessionId, out UserSession userSession)
        {
            // Initialize
            questStatus status = null;

            UserSessionMgr userSessionMgr = new UserSessionMgr();
            status = userSessionMgr.Read(userSessionId, out userSession);
            if (!questStatusDef.IsSuccess(status))
            {
                return (new questStatus(status.Severity, "Invalid user session"));
            }
            return (new questStatus(Severity.Success));
        }
        #endregion

        #endregion


        #region Private Methods
        /*==================================================================================================================================
         * Private Methods
         *=================================================================================================================================*/
        #endregion
    }
}
