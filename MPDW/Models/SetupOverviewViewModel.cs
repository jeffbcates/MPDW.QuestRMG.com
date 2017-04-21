using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MPDW.Models
{
    public class SetupOverviewViewModel : BaseUserSessionViewModel
    {
        public SetupOverviewViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public SetupOverviewViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
        }
    }
}