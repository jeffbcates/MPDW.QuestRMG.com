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
using Quest.Functional.Logging;
using Quest.Services.Business.Logging;


namespace Quest.MPDW.Controllers
{
    public class BaseController : Controller
    {
        #region Declarations
        /*==================================================================================================================================
         * Declarations
         *=================================================================================================================================*/
        private UserSession _userSession = null;
        private LogSetting _logSetting = null;
        private HTTPRequestLogsMgr _httpRequestLogsMgr = null;
        private PortalRequestLogsMgr _portalRequestLogsMgr = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
         * Constructors
         *=================================================================================================================================*/
        public BaseController()
        {
            initialize();
        }
        public BaseController(UserSession userSession)
        {
            this._userSession = userSession;
        }
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
        public NameValueCollection ParseQueryString(string queryString)
        {
            return(HttpUtility.ParseQueryString(queryString));
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
            questStatus status = null;

            status = Authorize();
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            status = logRequest();
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        #endregion


        #region Authorization
        /*==================================================================================================================================
         * Authorization
         *=================================================================================================================================*/
        public questStatus Authorize()
        {
            // NOTE: THIS CLASS HAS REDUNDANT CODE TO AVOID RE-TETSING ALREADY TESTED CODE. INSTEAD, ---EXTENDING--- THE CODE, TEMPORARILY, IS THE WAY TO GO.
            //       OPTIMIZE IT LATER.

            // If we have a user session context submitted, use it.  Otherwise, this is a non-user session authorization which could be anything, e.g.
            // user agent, IP address, etc.
            string _ctx = Request.QueryString["_ctx"];
            if (_ctx != null)
            {
                try
                {
                    int ictx = BaseId.INVALID_ID;
                    if (! int.TryParse(_ctx, out ictx))
                    {
                        return (new questStatus(Severity.Error, String.Format("Invalid user session")));
                    }
                    return (Authorize(ictx));
                }
                catch (System.Exception ex)
                {
                    return (new questStatus(Severity.Fatal, String.Format("Invalid user session")));
                }
            }
            else if (Request.HttpMethod.Equals("POST", StringComparison.InvariantCultureIgnoreCase))
            {
                NameValueCollection nvc = ParseQueryString(Request.UrlReferrer.AbsoluteUri);
                _ctx = nvc["_ctx"];
                if (_ctx != null)
                {
                    try
                    {
                        int ictx = BaseId.INVALID_ID;
                        if (!int.TryParse(_ctx, out ictx))
                        {
                            return (new questStatus(Severity.Error, String.Format("Invalid user session")));
                        }
                        return (Authorize(ictx));
                    }
                    catch (System.Exception ex)
                    {
                        return (new questStatus(Severity.Fatal, String.Format("Invalid user session")));
                    }
                }
            }
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
        private void initialize()
        {
            if (Request == null)
            {
                return;
            }
            loadLogSettings();
        }

        #region Logging
        /*----------------------------------------------------------------------------------------------------------------------------------
         * Logging
         *---------------------------------------------------------------------------------------------------------------------------------*/
        private questStatus loadLogSettings()
        {
            // Initialize
            questStatus status = null;


            // Get log settings.
            LogSetting logSetting = null;
            LogSettingsMgr logSettingsMgr = new LogSettingsMgr(this.UserSession);
            status = logSettingsMgr.Read(out logSetting);
            if (!questStatusDef.IsSuccess(status))
            {
                this._logSetting = new LogSetting();
                return (status);
            }
            this._logSetting = logSetting;

            if (this._logSetting.bLogHTTPRequests)
            {
                _httpRequestLogsMgr = new HTTPRequestLogsMgr(this.UserSession);
            }
            if (this._logSetting.bLogPortal)
            {
                _portalRequestLogsMgr = new PortalRequestLogsMgr(this.UserSession);
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus logRequest()
        {
            // Initialize
            questStatus status = null;


            // Log Portal request
            status = logPortalRequest();
            if (questStatusDef.IsSuccess(status))
            {
                // Do NOT log both portal and HTTP request with a portal request.
                return (status);
            }

            // Log HTTP request
            status = logHTTPRequest();
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus logPortalRequest()
        {
            // Initialize
            questStatus status = null;


            // If the custom controller factory invoked us w/o a Request context,
            // we need to load log settings.
            if (this._logSetting == null)
            {
                loadLogSettings();
            }
            if (! this._logSetting.bLogPortal)
            {
                // Portal logging not turned on, thus return WARNING so HTTP request can be logged.
                return (new questStatus(Severity.Warning));
            }

            // If there is no agent, nothing to log.
            if (Request.QueryString["_Agent"] == null)
            {
                return (new questStatus(Severity.Warning));
            }

            // Log portal request.
            PortalRequestLog portalRequestLog = new PortalRequestLog();
            if (this.UserSession != null)
            {
                portalRequestLog.UserSessionId = this.UserSession == null ? -1 : this.UserSession.Id;
                portalRequestLog.Username = this.UserSession.User.Username;
            }
            portalRequestLog.Name = Request.QueryString["_Agent"].ToString().Replace("\"", "");
            portalRequestLog.Method = Request.HttpMethod;
            portalRequestLog.IPAddress = Request.UserHostAddress;
            portalRequestLog.UserAgent = Request.UserAgent;
            portalRequestLog.URL = Request.Url.ToString();
            PortalRequestLogId portalRequestLogId = null;
            status = _portalRequestLogsMgr.Create(portalRequestLog, out portalRequestLogId);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        private questStatus logHTTPRequest()
        {
            // Initialize
            questStatus status = null;


            if (this._logSetting.bLogHTTPRequests)
            {
                HTTPRequestLog httpRequestLog = new HTTPRequestLog();
                if (this.UserSession != null)
                {
                    httpRequestLog.UserSessionId = this.UserSession == null ? -1 : this.UserSession.Id;
                    httpRequestLog.Username = this.UserSession.User.Username;
                }
                httpRequestLog.Method = Request.HttpMethod;
                httpRequestLog.IPAddress = Request.UserHostAddress;
                httpRequestLog.UserAgent = Request.UserAgent;
                httpRequestLog.URL = Request.Url.ToString();
                HTTPRequestLogId httpRequestLogId = null;
                status = _httpRequestLogsMgr.Create(httpRequestLog, out httpRequestLogId);
                if (!questStatusDef.IsSuccess(status))
                {
                    return (status);
                }
            }
            return (new questStatus(Severity.Success));
        }
        #endregion


        #region User Session
        /*----------------------------------------------------------------------------------------------------------------------------------
         * User Session
         *---------------------------------------------------------------------------------------------------------------------------------*/
        public questStatus GetRequestContext()
        {
            string _ctx = Request.QueryString["_ctx"];
            if (_ctx != null)
            {
                try
                {
                    int ictx = BaseId.INVALID_ID;
                    if (!int.TryParse(_ctx, out ictx))
                    {
                        return (new questStatus(Severity.Error, String.Format("Invalid user session")));
                    }
                }
                catch (System.Exception)
                {
                    return (new questStatus(Severity.Fatal, String.Format("Invalid user session")));
                }
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus LoadUserSession(int ictx)
        {
            // Initialize
            questStatus status = null;


            // Get user session and assign as property to this class.
            UserSessionId userSessionId = new UserSessionId(ictx);
            UserSession userSession = null;
            status = ValidateUserSession(userSessionId, out userSession);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            this._userSession = userSession;


            return (new questStatus(Severity.Success));
        }
        #endregion

        #endregion
    }
}
