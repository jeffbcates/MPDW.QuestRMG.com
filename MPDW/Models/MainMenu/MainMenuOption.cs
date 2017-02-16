using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Quest.MPDW.Models.Menus
{
    public class MainMenuOption
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public MainMenuOptionType MenuOptionType { get; set; }
        public string Uri { get; set; }
        public string Method { get; set; }
        public string Tooltip { get; set; }
        public bool bCurrentItem { get; set; }
        public bool bClientEvent { get; set; }
        public List<MainMenuArgument> Arguments { get; set; }
        public List<MainMenuOption> Options { get; set; }

        public MainMenuOption()
        {
            Options = new List<MainMenuOption>();
            Arguments = new List<MainMenuArgument>();
        }
        public MainMenuOption(string name)
            : this()
        {
            Name = name;
            Label = name;
        }
        public MainMenuOption(string label, string name)
            : this()
        {
            Label = label;
            Name = name;
        }
        public MainMenuOption(string label, string name, string toolTip)
            : this()
        {
            Label = label;
            Name = name;
            Tooltip = toolTip;
        }
        public MainMenuOption(string label, string name, string toolTip, string uri)
            : this()
        {
            Label = label;
            Name = name;
            Tooltip = toolTip;
            Uri = uri;
        }
        public MainMenuOption(string label, string name, string toolTip, string uri, bool clientEvent)
            : this()
        {
            Label = label;
            Name = name;
            Tooltip = toolTip;
            Uri = uri;
            bClientEvent = clientEvent;
        }
    }
}