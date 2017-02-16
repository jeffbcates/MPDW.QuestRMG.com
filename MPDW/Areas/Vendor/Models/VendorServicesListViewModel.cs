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
    public class VendorServicesListViewModel : VendorBaseViewModel
    {
        public List<VendorServicesListViewModel> Items { get; set; }
        public QueryOptions QueryOptions { get; set; }


        public VendorServicesListViewModel(UserSession userSession)
            : base(userSession)
        {
            Items = new List<VendorServicesListViewModel>();
            QueryOptions = new QueryOptions();
        }
        public VendorServicesListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Items = new List<VendorServicesListViewModel>();
            QueryOptions = new QueryOptions();
        }
    }
}