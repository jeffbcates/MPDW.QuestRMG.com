using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MPDW.Admin.Models
{
    public class AdminBaseViewModel : BaseUserSessionViewModel
    {
        public AdminBaseViewModel() { }
        public AdminBaseViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public AdminBaseViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
        }
    }
}