using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MasterPricing.Database.Models
{
    public class StoredProceduresListViewModel : DatabaseBaseListViewModel
    {
        public int DatabaseId { get; set; }
        public List<StoredProcedureLineItemViewModel> Items { get; set; }


        public StoredProceduresListViewModel()
        {
            Items = new List<StoredProcedureLineItemViewModel>();
        }
        public StoredProceduresListViewModel(UserSession userSession)
            : base(userSession)
        {
            Items = new List<StoredProcedureLineItemViewModel>();
        }
        public StoredProceduresListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Items = new List<StoredProcedureLineItemViewModel>();
        }
    }
}