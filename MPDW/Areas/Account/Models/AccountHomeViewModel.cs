using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MPDW.Account.Models
{
    public class AccountHomeViewModel : BaseUserSessionViewModel
    {
        public AccountHomeViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public AccountHomeViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
        }
    }
}