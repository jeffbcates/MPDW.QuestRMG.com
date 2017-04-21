using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MPDW.Support.Models
{
    public class ExceptionsViewModel : BaseUserSessionViewModel
    {
        public ExceptionsViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public ExceptionsViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
        }
    }
}