using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using Quest.Util.Status;
using Quest.Util.Buffer;
using Quest.Util.Data;
using Quest.Functional.FMS;
using Quest.MPDW.Models.Menus;



namespace Quest.MPDW.Models
{
    public class UserSessionViewModel 
    {
        public int Id { get; set; }
        public UserViewModel User { get; set; }
        public ProfileTypeId ProfileTypeId { get; set; }
        public string IPAddress { get; set; }
        public string UserAgent { get; set; }
        public DateTime Created { get; set; }


        public UserSessionViewModel()
        {
            User = new UserViewModel();
            ProfileTypeId = new ProfileTypeId(BaseId.INVALID_ID);
        }    
    }
}