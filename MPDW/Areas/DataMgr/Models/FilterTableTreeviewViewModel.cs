using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MasterPricing.DataMgr.Models
{
    public class FilterTableTreeviewViewModel : DataMgrBaseViewModel
    {
        public int TablesetId { get; set; }

        public FilterTableTreeviewViewModel()
            : base()
        {
        }
        public FilterTableTreeviewViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public FilterTableTreeviewViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
        } 
    }
}