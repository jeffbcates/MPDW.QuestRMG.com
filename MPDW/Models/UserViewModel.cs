using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using Quest.Util.Status;
using Quest.Functional.FMS;
using Quest.MPDW.Models.Menus;


namespace Quest.MPDW.Models
{
    public class UserViewModel 
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}