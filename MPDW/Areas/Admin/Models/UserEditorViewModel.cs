using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.Util.Data;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MPDW.Admin.Models
{
    public class UserEditorViewModel : AdminBaseViewModel
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Username { get; set; }
        public bool bEnabled { get; set; }
        public bool bActive { get; set; }
        public bool bLogSession { get; set; }


        public UserEditorViewModel()
            : base()
        {
        }
        public UserEditorViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public UserEditorViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
        }
    }
}