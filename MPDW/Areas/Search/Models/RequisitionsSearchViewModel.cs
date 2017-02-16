using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MPDW.Search.Models
{
    public class RequisitionsSearchViewModel : SearchBaseViewModel
    {
        public RequisitionsSearchViewModel() { }
        public RequisitionsSearchViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public RequisitionsSearchViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
        }
    }
}