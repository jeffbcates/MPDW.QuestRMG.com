using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.Util.Status;


namespace Quest.MPDW.Models.Menus
{
    public class MainMenu
    {
        public List<MainMenuOption> Options { get; set; }

        public MainMenu()
        {
            Options = new List<MainMenuOption>();
        }
        public MainMenu(string currentMenuItem)
        {
            // TODO: parse path and set item's .bCurrentItem = true.
        }


        public questStatus SetCurrentItem(string currentItem)
        {
            // TODO: parse path and set item's .bCurrentItem = true.

            return (new questStatus(Severity.Warning, "Not implemented yet"));
        }
    }
}