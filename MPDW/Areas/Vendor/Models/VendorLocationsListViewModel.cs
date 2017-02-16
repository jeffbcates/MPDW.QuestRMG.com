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
    public class VendorLocationsListViewModel : VendorBaseViewModel
    {
        public List<VendorLocationsListViewModel> Items { get; set; }
        public QueryOptions QueryOptions { get; set; }


        public VendorLocationsListViewModel(UserSession userSession)
            : base(userSession)
        {
            Items = new List<VendorLocationsListViewModel>();
            QueryOptions = new QueryOptions();
        }
        public VendorLocationsListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Items = new List<VendorLocationsListViewModel>();
            QueryOptions = new QueryOptions();
        }
    }
}