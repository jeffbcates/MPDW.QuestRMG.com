using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.Functional.MasterPricing;


namespace Quest.MasterPricing.Database.Models
{
    public class TablesListViewModel : DatabaseBaseListViewModel
    {
        public int DatabaseId { get; set; }
        public DatabaseBaseViewModel Database { get; set; }
        public List<TableLineItemViewModel> Items { get; set; }


        public TablesListViewModel()
        {
            Database = new DatabaseBaseViewModel();
            Items = new List<TableLineItemViewModel>();
        }
        public TablesListViewModel(UserSession userSession)
            : base(userSession)
        {
            Database = new DatabaseBaseViewModel();
            Items = new List<TableLineItemViewModel>();
        }
        public TablesListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Database = new DatabaseBaseViewModel();
            Items = new List<TableLineItemViewModel>();
        }
    }
}