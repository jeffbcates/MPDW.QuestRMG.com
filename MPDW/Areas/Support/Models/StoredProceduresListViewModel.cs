using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.Util.Data;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MPDW.Support.Models
{
    public class StoredProceduresListViewModel : SupportBaseListViewModel
    {
        public List<StoredProcedureLineItemViewModel> Items { get; set; }


        public StoredProceduresListViewModel()
            : base()
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