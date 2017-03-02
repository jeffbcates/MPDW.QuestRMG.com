using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MasterPricing.DataMgr.Models
{
    public class FilterCopyViewModel : DataMgrBaseViewModel
    {
        public int FilterId { get; set; }
        public int NewFilterId { get; set; }

        public FilterCopyViewModel()
            : base()
        {
        }
        public FilterCopyViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public FilterCopyViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
        }
    }
}