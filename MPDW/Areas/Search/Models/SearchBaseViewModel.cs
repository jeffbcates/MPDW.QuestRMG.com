using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MPDW.Search.Models
{
    public class SearchBaseViewModel : BaseUserSessionViewModel
    {
        public SearchBaseViewModel() { }
        public SearchBaseViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public SearchBaseViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
        }
    }
}