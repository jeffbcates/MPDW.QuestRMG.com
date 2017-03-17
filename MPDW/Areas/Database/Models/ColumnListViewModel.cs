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
    public class ColumnListViewModel : DatabaseBaseListViewModel
    {
        public int DatabaseId { get; set; }
        public DatabaseBaseViewModel Database { get; set; }
        public string ParentEntityType { get; set; }
        public TableViewModel Table { get; set; }
        public ViewViewModel View { get; set; }
        public List<ColumnLineItemViewModel> Items { get; set; }


        public ColumnListViewModel()
        {
            Database = new DatabaseBaseViewModel();
            Table = new TableViewModel();
            View = new ViewViewModel();
            Items = new List<ColumnLineItemViewModel>();
        }
        public ColumnListViewModel(UserSession userSession)
            : base(userSession)
        {
            Database = new DatabaseBaseViewModel();
            Table = new TableViewModel();
            View = new ViewViewModel();
            Items = new List<ColumnLineItemViewModel>();
        }
        public ColumnListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Database = new DatabaseBaseViewModel();
            Table = new TableViewModel();
            View = new ViewViewModel();
            Items = new List<ColumnLineItemViewModel>();
        }
    }
}