using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.Util.Status;


namespace Quest.MPDW.Models.Menus
{
    public class Navbar
    {
        #region Declarations
        /*==================================================================================================================================
        * Declarations
        *=================================================================================================================================*/
        public List<NavbarOption> Options { get; set; }
        public string CurrentItem { get; set; }
        public bool bExpanded { get; set; }

        #endregion


        #region Constructors
        /*==================================================================================================================================
        * Constructors
        *=================================================================================================================================*/
        public Navbar()
        {
            Options = new List<NavbarOption>();
            CurrentItem = "";
            bExpanded = false;
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
        public questStatus SetCurrentItem(string currentItem)
        {
            NavbarOption navbarOption = GetOption(currentItem);
            if (navbarOption != null)
            {
                navbarOption.bCurrentItem = true;
            }
            else
            {
                return (new questStatus(Severity.Warning, "Option not found"));
            }
            return (new questStatus(Severity.Success));
        }
        public NavbarOption GetOption(string currentItem, int idx = -1, List<NavbarOption> options = null)
        {
            NavbarOption _navbarOption = null;

            char[] delimiters = { '/' };
            string[] _parts = currentItem.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            int _idx = idx == -1 ? 0 : idx;
            List<NavbarOption> _options = options != null ? options : this.Options;

            foreach (NavbarOption navbarOption in _options)
            {
                if (_parts[_idx] == navbarOption.Name)
                {
                    if ((_idx + 1) < _parts.Length)
                    {
                        if (navbarOption.Options.Count > 0)
                        {
                            _navbarOption = GetOption(currentItem, _idx + 1, navbarOption.Options);
                            if (_navbarOption != null)
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        _navbarOption = navbarOption;
                        break;
                    }
                }
            }
            return (_navbarOption);
        }
        #endregion
    }
}