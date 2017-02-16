using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.Util.Status;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MPDW.Models
{
    public class AdminViewModel : BaseUserSessionViewModel
    {
        public AdminViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public AdminViewModel(UserSession userSession, BaseViewModel baseViewModel)
            : base(userSession, baseViewModel)
        {
        }
    }
}