using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MasterPricing.Database.Models
{
    public class TablesListViewModel : DatabaseBaseListViewModel
    {
        public int DatabaseId { get; set; }
        public List<TableLineItemViewModel> Items { get; set; }


        public TablesListViewModel()
        {
            Items = new List<TableLineItemViewModel>();
        }
        public TablesListViewModel(UserSession userSession)
            : base(userSession)
        {
            Items = new List<TableLineItemViewModel>();
        }
        public TablesListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Items = new List<TableLineItemViewModel>();
        }
    }
}