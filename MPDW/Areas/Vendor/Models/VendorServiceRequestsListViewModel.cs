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
    public class VendorServiceRequestsListViewModel : VendorBaseViewModel
    {
        public List<VendorServiceRequestsListViewModel> Items { get; set; }
        public QueryOptions QueryOptions { get; set; }


        public VendorServiceRequestsListViewModel(UserSession userSession)
            : base(userSession)
        {
            Items = new List<VendorServiceRequestsListViewModel>();
            QueryOptions = new QueryOptions();
        }
        public VendorServiceRequestsListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Items = new List<VendorServiceRequestsListViewModel>();
            QueryOptions = new QueryOptions();
        }
    }
}