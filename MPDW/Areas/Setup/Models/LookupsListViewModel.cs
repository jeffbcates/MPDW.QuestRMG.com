using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.Util.Data;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MasterPricing.Setup.Models
{
    public class LookupsListViewModel : SetupBaseListViewModel
    {
        public List<LookupLineItemViewModel> Items { get; set; }


        public LookupsListViewModel()
            : base()
        {
            Items = new List<LookupLineItemViewModel>();
        }
        public LookupsListViewModel(UserSession userSession)
            : base(userSession)
        {
            Items = new List<LookupLineItemViewModel>();
        }
        public LookupsListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Items = new List<LookupLineItemViewModel>();
        }
    }
}