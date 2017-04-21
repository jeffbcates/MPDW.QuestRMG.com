using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MasterPricing.DataMgr.Models
{
    public class DataMgrBaseViewModel : BaseUserSessionViewModel
    {
        public DataMgrBaseViewModel() { }
        public DataMgrBaseViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public DataMgrBaseViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
        }
    }
}