using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MasterPricing.DataMgr.Models
{
    public class TablesetListViewModel : DataMgrBaseListViewModel
    {
        public List<TablesetLineItemViewModel> Items { get; set; }


        public TablesetListViewModel()
        {
            Items = new List<TablesetLineItemViewModel>();
        }
        public TablesetListViewModel(UserSession userSession)
            : base(userSession)
        {
            Items = new List<TablesetLineItemViewModel>();
        }
        public TablesetListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Items = new List<TablesetLineItemViewModel>();
        }
    }
}