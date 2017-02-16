using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.MPDW.Models;
using Quest.MPDW.Modelers;
using Quest.MPDW.Models.Menus;
using Quest.MPDW.Services.Data;
using Quest.MPDW.Services.Business.Accounts;


namespace Quest.MPDW.Modelers
{
    public class BaseModeler
    {
        #region Declarations
        /*==================================================================================================================================
        * Declarations
        *=================================================================================================================================*/
        private UserSession _userSession = null;
        private HttpRequestBase _httpRequestBase = null;
        private MainMenu _mainMenu = null;
        private Navbar _navbar = null;

        #endregion


        #region Constructors
        /*==================================================================================================================================
        * Constructors
        *=================================================================================================================================*/
        public BaseModeler(HttpRequestBase httpRequestBase)
        {
            this._httpRequestBase = httpRequestBase;
            initialize();
        }
        public BaseModeler(HttpRequestBase httpRequestBase, UserSession userSession)
        {
            _httpRequestBase = httpRequestBase;
            _userSession = userSession;
            initialize();
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
        public MainMenu MainMenu
        {
            get
            {
                return (this._mainMenu);
            }
        }
        public Navbar Navbar
        {
            get
            {
                return (this._navbar);
            }
        }
        public HttpRequestBase HttpRequestBase
        {
            get
            {
                return (_httpRequestBase);
            }
        }
        #endregion


        #region Public Methods
        /*==================================================================================================================================
        * Public Methods
        *=================================================================================================================================*/
        public string GetVersion()
        {
            string _version = "v.???";

            if (ConfigurationManager.AppSettings.AllKeys.Contains("_Version"))
            {
                _version = ConfigurationManager.AppSettings.Get("_Version").ToString();
            }
            return (_version);
        }
        public questStatus BuildInvalidFieldList(BaseViewModel viewModel, List<ValidationResult> validationResultList)
        {
            List<string> invalidFields = new List<string>();
            List<string> userMessages = new List<string>();
            foreach (ValidationResult validationResult in validationResultList)
            {
                invalidFields.Add(string.Join(",", validationResult.MemberNames));
                userMessages.Add(validationResult.ErrorMessage);
            }
            viewModel.InvalidFields = invalidFields;
            viewModel.UserMessages = userMessages;
            return (new questStatus(Severity.Success));
        }
        public questStatus GetIPAddress(HttpRequestBase request, out string IPAddress)
        {
            // Initialize
            IPAddress = null;

            // Avoid proxy IP's.
            // TODO: have the client send in their public IP via client-side script.
            string ipAddress;
            try
            {
                ipAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(ipAddress))
                {
                    if (ipAddress.IndexOf(",") > 0)
                    {
                        string[] ipRange = ipAddress.Split(',');
                        int le = ipRange.Length - 1;
                        ipAddress = ipRange[le];
                    }
                }
                else
                {
                    ipAddress = request.UserHostAddress;
                }
            }
            catch { ipAddress = "0.0.0.0"; }
            IPAddress = ipAddress;

            return (new questStatus(Severity.Success));
        }


        #region Options
        //----------------------------------------------------------------------------------------------------------------------------------
        // Options
        //----------------------------------------------------------------------------------------------------------------------------------
        public questStatus AddDefaultOptions(List<OptionValuePair> optionsList, string Id = "-1", string Label = "")
        {
            // Initialize
            OptionValuePair defaultOptionValuePair = new OptionValuePair();
            defaultOptionValuePair.Id = Id;
            defaultOptionValuePair.Label = Label;
            optionsList.Insert(0, defaultOptionValuePair);

            return (new questStatus(Severity.Success));
        }
        #endregion


        #region Validations
        //----------------------------------------------------------------------------------------------------------------------------------
        // Validations
        //----------------------------------------------------------------------------------------------------------------------------------
        #endregion


        #region Error Messages
        //----------------------------------------------------------------------------------------------------------------------------------
        // Error Messages
        //----------------------------------------------------------------------------------------------------------------------------------
        public questStatus FormatErrorMessage(questStatus status, BaseViewModel baseViewModel)
        {
            // if MissingRequiredDataMember, DbUpdateException (EF6), DbEntityValidationException(EF5) -> data validation errors
            // else return the status as-is.
            if (status.Message.StartsWith("MissingRequiredDataMember: "))
            {
                string[] tokens = status.Message.Substring("MissingRequiredDataMember: ".Length).Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                if (tokens != null && tokens.Length > 1)
                {
                    baseViewModel.InvalidFields = new List<string>();
                    baseViewModel.UserMessages = new List<string>();
                    string[] fieldList = tokens[1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string field in fieldList)
                    {
                        questStatus errorStatus = new questStatus(Severity.Error, String.Format("The {0} field is required.|{1}", field, field));
                        baseViewModel.InvalidFields.Add(field);
                        UserMessageModeler userMessageModeler = new UserMessageModeler(errorStatus);
                        baseViewModel.UserMessages.Add(userMessageModeler.UserMessage);
                    }
                }
            }
            else if (status.Message.StartsWith("DbUpdateException: "))
            {
                status.Message = status.Message.Substring("DbUpdateException: ".Length);
                string[] errorMessageList = status.Message.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (errorMessageList != null && errorMessageList.Length > 1)
                {
                    string[] errorMessageTokens = errorMessageList[0].Split(new char[] { '\'' }, StringSplitOptions.RemoveEmptyEntries);
                    baseViewModel.InvalidFields = new List<string>();
                    baseViewModel.UserMessages = new List<string>();

                    questStatus errorStatus = new questStatus(Severity.Error, String.Format("The {0} field is required.|{1}", errorMessageTokens[1], errorMessageTokens[1]));
                    baseViewModel.InvalidFields.Add(errorMessageTokens[1]);
                    UserMessageModeler userMessageModeler = new UserMessageModeler(errorStatus);
                    baseViewModel.UserMessages.Add(userMessageModeler.UserMessage);
                }
            }
            else if (status.Message.StartsWith("DbEntityValidationException: "))
            {
                baseViewModel.InvalidFields = new List<string>();
                baseViewModel.UserMessages = new List<string>();
                string[] errorMessageList = status.Message.Substring("DbEntityValidationException: ".Length).Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string errorMessage in errorMessageList)
                {
                    string[] errorMessageTokens = errorMessage.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    questStatus errorStatus = new questStatus(Severity.Error, String.Format("{0}|{1}", FormatValidationErrorMessage(errorMessage), errorMessageTokens[1]));
                    baseViewModel.InvalidFields.Add(errorMessageTokens[1]);
                    UserMessageModeler userMessageModeler = new UserMessageModeler(errorStatus);
                    baseViewModel.UserMessages.Add(userMessageModeler.UserMessage);
                }
            }
            return (status);
        }
        public string FormatValidationErrorMessage(string errorMessage)
        {
            // Trim and get rid of a period at end (if any)
            string _errorMessage = errorMessage.Trim();
            if (_errorMessage.EndsWith("."))
            {
                _errorMessage = _errorMessage.Substring(0, _errorMessage.Length - 1);
            }
            string[] errorMessageTokens = _errorMessage.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string field = errorMessageTokens[1];

            //Get rid of starting b (if any)
            if (field.StartsWith("b"))
            {
                field = field.Substring(1);
            }
            // Camelcase to sentence structure
            field = Regex.Replace(field, "(\\B[A-Z])", " $1");
            errorMessageTokens[1] = field;
            string newErrorMessage = "";
            foreach (var x in errorMessageTokens)
            {
                newErrorMessage += x + " ";
            }
            newErrorMessage = newErrorMessage.Trim();

            return newErrorMessage;
        }
        #endregion


        #region Menu Contexts
        //----------------------------------------------------------------------------------------------------------------------------------
        // Menu Contexts
        //----------------------------------------------------------------------------------------------------------------------------------
        public questStatus ParseMainMenuContext()
        {
            // Initialize
            char[] delimiters = { '[', ']' };


            // Parse
            try
            {
                MainMenu _mainMenu = new MainMenu();
                for (int index = 0; index < _httpRequestBase.QueryString.AllKeys.Length; index++)
                {
                    string key = _httpRequestBase.QueryString.AllKeys[index];

                    string[] parts = key.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 0) { continue; }
                    if (parts[0] == "mm")
                    {
                        if (parts[1] == "ctx")
                        {
                            string CurrentItem = _httpRequestBase.QueryString[index];

                            // TODO: set bCurrentItem = true in MainMenu
                            _mainMenu.SetCurrentItem(CurrentItem);
                        }
                    }
                }
                this._mainMenu = _mainMenu;
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Error, String.Format("Error parsing MainMenu context: " + ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus ParseNavbarContext()
        {
            // Initialize
            questStatus status = null;
            char[] delimiters = { '[', ']' };

            // Parse
            try
            {
                Navbar _navbar = new Navbar();
                for (int index = 0; index < _httpRequestBase.QueryString.AllKeys.Length; index++)
                {
                    string key = _httpRequestBase.QueryString.AllKeys[index];
                    string[] parts = key.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 0) { continue; }
                    if (parts[0] == "nb")
                    {
                        if (parts[1] == "ctx")
                        {
                            string CurrentItem = _httpRequestBase.QueryString[index];

                            // TODO: set bCurrentItem = true in MainMenu
                            status = _navbar.SetCurrentItem(CurrentItem);
                            if (questStatusDef.IsWarning(status))
                            {
                                _navbar.CurrentItem = CurrentItem;
                            }
                        }
                    }
                }
                this._navbar = _navbar;
            }
            catch (System.Exception ex)
            {
                return (new questStatus(Severity.Error, String.Format("Error parsing Navbar context: " + ex.Message)));
            }
            return (new questStatus(Severity.Success));
        }
        #endregion

        #endregion


        #region Private Methods
        /*==================================================================================================================================
        * Private Methods
        *=================================================================================================================================*/
        private questStatus initialize()
        {
            ParseMainMenuContext();
            ParseNavbarContext();
            return (new questStatus(Severity.Success));
        }
        #endregion
    }

}