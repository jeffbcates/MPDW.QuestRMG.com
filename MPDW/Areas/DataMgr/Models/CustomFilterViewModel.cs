using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MasterPricing.DataMgr.Models
{
    public class CustomFilterViewModel : DataMgrBaseViewModel
    {

        public CustomFilterViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public CustomFilterViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
        } 
    }
}