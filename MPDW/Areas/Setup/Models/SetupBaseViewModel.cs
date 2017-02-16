using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MasterPricing.Setup.Models
{
    public class SetupBaseViewModel : BaseUserSessionViewModel
    {

        public SetupBaseViewModel() { }
        public SetupBaseViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public SetupBaseViewModel(UserSession userSession, BaseViewModel baseViewModel)
            : base(userSession, baseViewModel)
        {
        }
    }
}