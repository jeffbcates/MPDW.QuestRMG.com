using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MasterPricing.Filter.Models
{
    public class FilterBaseViewModel : BaseUserSessionViewModel
    {
        public FilterBaseViewModel() { }
        public FilterBaseViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public FilterBaseViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
        }
    }
}