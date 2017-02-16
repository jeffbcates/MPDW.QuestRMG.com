using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Quest.MPDW.Models.Menus
{
    public class NavbarOption
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public NavbarOptionType NavbarOptionType { get; set; }
        public string Uri { get; set; }
        public string Method { get; set; }
        public string Tooltip { get; set; }
        public bool bCurrentItem { get; set; }
        public bool bClientEvent { get; set; }
        public List<NavbarArgument> Arguments { get; set; }
        public List<NavbarOption> Options { get; set; }

        public NavbarOption()
        {
            Options = new List<NavbarOption>();
            Arguments = new List<NavbarArgument>();
        }
        public NavbarOption(string name)
            : this()
        {
            Name = name;
            Label = name;
        }
        public NavbarOption(string label, string name)
            : this()
        {
            Label = label;
            Name = name;
        }
        public NavbarOption(string label, string name, string toolTip)
            : this()
        {
            Label = label;
            Name = name;
            Tooltip = toolTip;
        }
        public NavbarOption(string label, string name, string toolTip, string uri)
            : this()
        {
            Label = label;
            Name = name;
            Tooltip = toolTip;
            Uri = uri;
        }
        public NavbarOption(string label, string name, string toolTip, string uri, bool clientEvent)
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