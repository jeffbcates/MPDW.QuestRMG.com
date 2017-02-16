using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Quest.MPDW.Models.Menus
{
    public class NavbarArgument
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public NavbarArgument()
        {
        }
        public NavbarArgument(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}