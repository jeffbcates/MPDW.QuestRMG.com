using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.Functional.MasterPricing;
using Quest.MPDW.Models.List;


namespace Quest.MasterPricing.DataMgr.Models
{
    public class DataMgrBaseListViewModel : DataMgrBaseViewModel
    {
        public QueryOptionsViewModel QueryOptions { get; set; }
        public QueryResponseViewModel QueryResponse { get; set; }

        public DataMgrBaseListViewModel() 
        {
            QueryOptions = new QueryOptionsViewModel();
            QueryResponse = new QueryResponseViewModel();
        }
        public DataMgrBaseListViewModel(UserSession userSession)
            : base(userSession)
        {
            QueryOptions = new QueryOptionsViewModel();
            QueryResponse = new QueryResponseViewModel();
        }
        public DataMgrBaseListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            QueryOptions = new QueryOptionsViewModel();
            QueryResponse = new QueryResponseViewModel();
        }
    }
}