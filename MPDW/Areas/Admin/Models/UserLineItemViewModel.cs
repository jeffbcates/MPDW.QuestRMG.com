using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Quest.MPDW.Admin.Models
{
    public class UserLineItemViewModel
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Username { get; set; }
        public bool bEnabled { get; set; }
        public bool bActive { get; set; }
    }
}