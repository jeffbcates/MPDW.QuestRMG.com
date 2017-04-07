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
using Quest.MPDW.Models;
using Quest.MPDW.Support.Models;
using Quest.MPDW.Services.Business.Accounts;
using Quest.Services.Business.Logging;


namespace Quest.MPDW.Support.Modelers
{
    public class LogSettingsEditorModeler : SupportBaseModeler
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
        public LogSettingsEditorModeler(HttpRequestBase httpRequestBase, UserSession userSession)
            : base(httpRequestBase, userSession)
        {
            initialize();
        }
        #endregion


        #region Public Methods
        /*==================================================================================================================================
        * Public Methods
        *=================================================================================================================================*/

        #region CRUD 
        //----------------------------------------------------------------------------------------------------------------------------------
        // CRUD
        //----------------------------------------------------------------------------------------------------------------------------------
        public questStatus Read(BaseUserSessionViewModel baseUserSessionViewModel, out LogSettingsViewModel logSettingsViewModel)
        {
            // Initialize
            questStatus status = null;
            logSettingsViewModel = null;


            // Read
            Quest.Functional.Logging.LogSetting logSetting = null;
            LogSettingsMgr logSettingsMgr = new LogSettingsMgr(this.UserSession);
            status = logSettingsMgr.Read(out logSetting);
            if (!questStatusDef.IsSuccess(status))
            {
                return (status);
            }

            // Transfer model.
            logSettingsViewModel = new LogSettingsViewModel(this.UserSession, baseUserSessionViewModel);
            BufferMgr.TransferBuffer(logSetting, logSettingsViewModel);


            return (new questStatus(Severity.Success));
        }
        public questStatus Save(LogSettingsViewModel logSettingsViewMode)
        {
            // Initialize
            questStatus status = null;


            // Transfer model
            Quest.Functional.Logging.LogSetting logSetting = new Functional.Logging.LogSetting();
            BufferMgr.TransferBuffer(logSettingsViewMode, logSetting);


            // Update
            LogSettingsMgr logSettingsMgr = new LogSettingsMgr(this.UserSession);
            status = logSettingsMgr.Update(logSetting);
            if (!questStatusDef.IsSuccess(status))
            {
                FormatErrorMessage(status, logSettingsViewMode);
                return (status);
            }
            return (new questStatus(Severity.Success));
        }
        #endregion


        #region Options
        //----------------------------------------------------------------------------------------------------------------------------------
        // Options
        //----------------------------------------------------------------------------------------------------------------------------------
        #endregion


        #region Validations
        //----------------------------------------------------------------------------------------------------------------------------------
        // Validations
        //----------------------------------------------------------------------------------------------------------------------------------
        #endregion

        #endregion


        #region Private Methods
        /*==================================================================================================================================
        * Private Methods
        *=================================================================================================================================*/
        private questStatus initialize()
        {
            return (new questStatus(Severity.Success));
        }
        #endregion
    }
}