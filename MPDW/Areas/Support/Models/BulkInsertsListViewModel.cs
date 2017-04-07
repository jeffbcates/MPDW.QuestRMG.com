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
    public class BulkInsertsListViewModel : SupportBaseListViewModel
    {
        public List<BulkInsertLineItemViewModel> Items { get; set; }


        public BulkInsertsListViewModel()
            : base()
        {
            Items = new List<BulkInsertLineItemViewModel>();
        }
        public BulkInsertsListViewModel(UserSession userSession)
            : base(userSession)
        {
            Items = new List<BulkInsertLineItemViewModel>();
        }
        public BulkInsertsListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Items = new List<BulkInsertLineItemViewModel>();
        }
    }
}