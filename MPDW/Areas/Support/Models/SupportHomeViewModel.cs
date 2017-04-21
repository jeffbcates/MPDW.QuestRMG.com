using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MPDW.Support.Models
{
    public class SupportHomeViewModel : BaseUserSessionViewModel
    {
        public SupportHomeViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public SupportHomeViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
        }
    }
}