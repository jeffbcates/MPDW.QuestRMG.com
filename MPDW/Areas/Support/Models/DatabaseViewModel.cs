using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MPDW.Support.Models
{
    public class DatabaseViewModel : BaseUserSessionViewModel
    {
        public DatabaseViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public DatabaseViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
        }
    }
}