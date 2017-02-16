using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.MPDW.Models.Menus;


namespace Quest.MPDW.Models
{
    public class BaseUserSessionViewModel : BaseViewModel
    {
        #region Declarations
        /*==================================================================================================================================
        * Declarations
        *=================================================================================================================================*/
        public int _ctx { get; set; }
        private UserSession _userSession = null;
        public UserSessionViewModel UserSession = null;
       
        #endregion


        #region Constructors
        /*==================================================================================================================================
        * Constructors
        *=================================================================================================================================*/
        public BaseUserSessionViewModel()
        { }
        public BaseUserSessionViewModel(UserSession UserSession)
        {
            this._userSession = UserSession;
            this._ctx = UserSession.Id;
            this.UserSession = new UserSessionViewModel();
            BufferMgr.TransferBuffer(this._userSession, this.UserSession, true);
            BufferMgr.TransferBuffer(this._userSession.User, this.UserSession.User);

            initialize();
        }
        public BaseUserSessionViewModel(UserSession UserSession, BaseViewModel baseViewModel)
            : base(baseViewModel)
        {
            this._userSession = UserSession;
            this._ctx = UserSession.Id;
            this.UserSession = new UserSessionViewModel();
            BufferMgr.TransferBuffer(this._userSession, this.UserSession, true);
            BufferMgr.TransferBuffer(this._userSession.User, this.UserSession.User);

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
        public questStatus SetUserSession(UserSession userSession)
        {
            this._userSession = userSession;

            return (new questStatus(Severity.Success));
        }
        #endregion


        #region Private Methods
        /*==================================================================================================================================
        * Private Methods
        *=================================================================================================================================*/
        private void initialize()
        {
            questStatus = new questStatus(Severity.Unknown, "");
            InvalidFields = new List<string>();
            UserMessages = new List<string>();

            if (this._userSession == null)
            {
                this._userSession = new UserSession();
                this._userSession.Id = BaseId.INVALID_ID;
                this._userSession.UserId = BaseId.INVALID_ID;
            }
            buildMainMenu();
        }
        private void buildMainMenu()
        {
            if (this._userSession != null)
            {

                // All users have these options.
                MainMenuOption mmoSetup = new MainMenuOption("Setup", "Setup", "Setup Tablesets", "/Setup");
                mmoSetup.MenuOptionType = MainMenuOptionType.MenuOption;
                MainMenu.Options.Add(mmoSetup);

                MainMenuOption mmoDataMgr = new MainMenuOption("Manage Data", "DataMgr", "Browse and change date", "/DataMgr");
                mmoDataMgr.MenuOptionType = MainMenuOptionType.MenuOption;
                MainMenu.Options.Add(mmoDataMgr);

                MainMenuOption mmoAdmin = new MainMenuOption("Admin", "Admin", "Administrative panel", "/Admin");
                mmoAdmin.MenuOptionType = MainMenuOptionType.MenuOption;
                MainMenu.Options.Add(mmoAdmin);
            }

        }
        #endregion
    }
}