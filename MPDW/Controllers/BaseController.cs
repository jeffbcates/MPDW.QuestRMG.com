using System;
using System.IO;
using System.Text;
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
using Quest.Functional.MasterPricing;
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

            UserSessionMgr userSessionMgr = new UserSessionMgr(this.UserSession);
            status = userSessionMgr.Read(userSessionId, out userSession);
            if (!questStatusDef.IsSuccess(status))
            {
                return (new questStatus(status.Severity, "Invalid user session"));
            }
            return (new questStatus(Severity.Success));
        }
        #endregion


        #region Export Routines
        /*==================================================================================================================================
         * Export Routines
         *=================================================================================================================================*/
        public void WriteTsv(ResultsSet resultsSet, TextWriter output)
        {
            foreach (KeyValuePair<string, Column> kvp in resultsSet.ResultColumns)
            {
                output.Write(kvp.Key); // header
                output.Write("\t");
            }
            output.WriteLine();
            foreach (dynamic _dynRow in resultsSet.Data)
            {
                foreach (KeyValuePair<string, object> kvp in _dynRow)
                {
                    if (kvp.Value != null && kvp.Value.ToString().IndexOf("This location does not want follow up calls effective") > -1)
                    {
                        int x = 4;
                        x = 44;
                    }
                    string value = kvp.Value == null ? "(null)" : kvp.Value.ToString().Replace("\t", " ").Replace("\r", " ").Replace("\n"," ");
                    output.Write(value);
                    output.Write("\t");

                }
                output.WriteLine();
            }
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
