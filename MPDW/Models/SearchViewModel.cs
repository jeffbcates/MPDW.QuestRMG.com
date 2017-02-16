using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MPDW.Models
{
    public class SearchViewModel : BaseUserSessionViewModel
    {
        public SearchViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public SearchViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            :base(userSession, baseUserSessionViewModel)
        {
        }
    }
}