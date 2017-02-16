using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Quest.MPDW.Models.Menus
{
    public class MainMenuArgument
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public MainMenuArgument()
        {
        }
        public MainMenuArgument(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}