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
    public class DatabasesListViewModel : SetupBaseListViewModel
    {
        public List<DatabaseLineItemViewModel> Items { get; set; }


        public DatabasesListViewModel()
            : base()
        {
            Items = new List<DatabaseLineItemViewModel>();
        }
        public DatabasesListViewModel(UserSession userSession)
            : base(userSession)
        {
            Items = new List<DatabaseLineItemViewModel>();
        }
        public DatabasesListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Items = new List<DatabaseLineItemViewModel>();
        }
    }
}