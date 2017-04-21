using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.Functional.MasterPricing;
using Quest.MPDW.Models.List;


namespace Quest.MasterPricing.Database.Models
{
    public class DatabaseBaseListViewModel : DatabaseBaseViewModel
    {
        public QueryOptionsViewModel QueryOptions { get; set; }
        public QueryResponseViewModel QueryResponse { get; set; }

        public DatabaseBaseListViewModel() 
        {
            QueryOptions = new QueryOptionsViewModel();
            QueryResponse = new QueryResponseViewModel();
        }
        public DatabaseBaseListViewModel(UserSession userSession)
            : base(userSession)
        {
            QueryOptions = new QueryOptionsViewModel();
            QueryResponse = new QueryResponseViewModel();
        }
        public DatabaseBaseListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            QueryOptions = new QueryOptionsViewModel();
            QueryResponse = new QueryResponseViewModel();
        }
    }
}