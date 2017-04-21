using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;
using Quest.Util.Status;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.MPDW.Models;
using Quest.MPDW.Services.Data;
using Quest.MPDW.Services.Business.Accounts;


namespace Quest.MPDW.Modelers
{
    public class UserSessionBaseModeler
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
        public UserSessionBaseModeler(UserSession userSession)
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
        #endregion


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

        #region Validations
        //----------------------------------------------------------------------------------------------------------------------------------
        // Validations
        //----------------------------------------------------------------------------------------------------------------------------------
        // TODO: VALIDATE MODELS FOR REQUIRED FIELDS IN HERE.
        #endregion

        #endregion




        #region Query Options
        //----------------------------------------------------------------------------------------------------------------------------------
        // Query Options
        //----------------------------------------------------------------------------------------------------------------------------------
        #region Paging/Sorting Support
        //
        // Paging/Sorting Support
        //
        #endregion

        #endregion


        #region Private Methods
        /*==================================================================================================================================
        * Private Methods
        *=================================================================================================================================*/
        private string getVersion()
        {
            string _version = "v.???";

            if (ConfigurationManager.AppSettings.AllKeys.Contains("_Version"))
            {
                _version = ConfigurationManager.AppSettings.Get("_Version").ToString();
            }
            return (_version);
        }
        #endregion
    }

}