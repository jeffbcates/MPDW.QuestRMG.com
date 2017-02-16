using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.Util.Data;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.MPDW.Models.List;


namespace Quest.MasterPricing.Setup.Models
{
    public class SetupBaseListViewModel : SetupBaseViewModel
    {
        public QueryOptionsViewModel QueryOptions { get; set; }
        public QueryResponseViewModel QueryResponse { get; set; }


        public SetupBaseListViewModel() { }
        public SetupBaseListViewModel(UserSession userSession)
            : base(userSession)
        {
            QueryOptions = new QueryOptionsViewModel();
            QueryResponse = new QueryResponseViewModel();
        }
        public SetupBaseListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            QueryOptions = new QueryOptionsViewModel();
            QueryResponse = new QueryResponseViewModel();
        }
    }
}