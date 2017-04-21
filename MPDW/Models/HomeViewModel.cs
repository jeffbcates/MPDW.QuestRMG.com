using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.Util.Status;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MPDW.Models
{
    public class HomeViewModel : BaseUserSessionViewModel
    {
        public HomeViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public HomeViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
        }
    }
}