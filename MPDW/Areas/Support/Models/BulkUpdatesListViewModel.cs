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
    public class BulkUpdatesListViewModel : SupportBaseListViewModel
    {
        public List<BulkUpdateLineItemViewModel> Items { get; set; }


        public BulkUpdatesListViewModel()
            : base()
        {
            Items = new List<BulkUpdateLineItemViewModel>();
        }
        public BulkUpdatesListViewModel(UserSession userSession)
            : base(userSession)
        {
            Items = new List<BulkUpdateLineItemViewModel>();
        }
        public BulkUpdatesListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Items = new List<BulkUpdateLineItemViewModel>();
        }
    }
}