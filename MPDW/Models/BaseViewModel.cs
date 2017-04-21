using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using Quest.Util.Status;
using Quest.MPDW.Models.Menus;


namespace Quest.MPDW.Models
{
    public class BaseViewModel
    {
        #region Declarations
        /*==================================================================================================================================
        * Declarations
        *=================================================================================================================================*/
        public string Version { get; set; }
        public questStatus questStatus { get; set; }
        public List<string> InvalidFields { get; set; }
        public List<string> UserMessages { get; set; }


        // Restore view contexts members.
        public string _mmI { get; set; }
        public string _nbI { get; set; }
        public string _nbX { get; set; }


        // Use these to server-side build options.
        private MainMenu _mainMenu = null;
        private Navbar _navbar = null;
        

        // Client directions
        public string _Action { get; set; }
        public string _Operation { get; set; }
        public string _URL { get; set; }

        #endregion


        #region Constructors
        /*==================================================================================================================================
        * Constructors
        *=================================================================================================================================*/
        public BaseViewModel()
        {
            initialize();
        }
        public BaseViewModel(BaseViewModel baseViewModel)
        {
            this._mmI = baseViewModel._mmI;
            this._nbI = baseViewModel._nbI;
            this._nbX = baseViewModel._nbX;
            initialize();
        }
        #endregion


        #region Properties
        /*==================================================================================================================================
        * Properties
        *=================================================================================================================================*/
        public virtual MainMenu MainMenu
        {
            get
            {
                return (_mainMenu);
            }
            set
            {
                _mainMenu = value;
            }
        }
        public virtual Navbar Navbar
        {
            get
            {
                return (_navbar);
            }
            set
            {
                _navbar = value;
            }
        }
        #endregion


        #region Public Methods
        /*==================================================================================================================================
        * Public Methods
        *=================================================================================================================================*/
        public void RestoreViewContext(BaseViewModel baseViewModel)
        {
            this._mmI = baseViewModel._mmI;
            this._nbI = baseViewModel._nbI;
            this._nbX = baseViewModel._nbX;
        }
        #endregion


        #region Private Methods
        /*==================================================================================================================================
        * Private Methods
        *=================================================================================================================================*/
        private void initialize()
        {
            Version = "v.20160921001";  // Temporary

            questStatus = new questStatus(Severity.Unknown, "");
            InvalidFields = new List<string>();
            UserMessages = new List<string>();

            buildMainMenu();
            buildNavbar();
        }
        private void buildMainMenu()
        {
            _mainMenu = new MainMenu();
        }
        private void buildNavbar()
        {
            _navbar = new Navbar();
        }
        #endregion
    }
}