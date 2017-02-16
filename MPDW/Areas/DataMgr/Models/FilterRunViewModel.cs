using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.Functional.MasterPricing;


namespace Quest.MasterPricing.DataMgr.Models
{
    public class FilterRunViewModel : DataMgrBaseListViewModel
    {
        public int Id { get; set; }
        public int FilterId { get; set; }
        public FilterResultsViewModel Results = null;


        public FilterRunViewModel()
            : base()
        {
            Results = new FilterResultsViewModel();
        }
        public FilterRunViewModel(UserSession userSession)
            : base(userSession)
        {
            Results = new FilterResultsViewModel();
        }
        public FilterRunViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Results = new FilterResultsViewModel();
        }
    }
}