using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MasterPricing.DataMgr.Models
{
    public class FilterSQLViewModel : DataMgrBaseViewModel
    {
        public int Id { get; set; }
        public string SQL { get; set; }


        public FilterSQLViewModel()
            : base()
        {
        }
        public FilterSQLViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public FilterSQLViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
        }
    }
}