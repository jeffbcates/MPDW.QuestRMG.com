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
    public class TablesetsListViewModel : SetupBaseListViewModel
    {
        public List<TablesetLineItemViewModel> Items { get; set; }


        public TablesetsListViewModel()
            : base()
        {
            Items = new List<TablesetLineItemViewModel>();
        }
        public TablesetsListViewModel(UserSession userSession)
            : base(userSession)
        {
            Items = new List<TablesetLineItemViewModel>();
        }
        public TablesetsListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Items = new List<TablesetLineItemViewModel>();
        }
    }
}