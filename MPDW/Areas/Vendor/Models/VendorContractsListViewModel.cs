using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.Util.Data;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MPDW.Vendor.Models
{
    public class VendorContractsListViewModel : VendorBaseViewModel
    {
        public List<VendorContractsListViewModel> Items { get; set; }
        public QueryOptions QueryOptions { get; set; }


        public VendorContractsListViewModel(UserSession userSession)
            : base(userSession)
        {
            Items = new List<VendorContractsListViewModel>();
            QueryOptions = new QueryOptions();
        }
        public VendorContractsListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Items = new List<VendorContractsListViewModel>();
            QueryOptions = new QueryOptions();
        }
    }
}