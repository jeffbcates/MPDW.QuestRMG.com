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
    public class VendorsListViewModel : VendorBaseViewModel
    {
        public List<VendorsListViewModel> Items { get; set; }
        public QueryOptions QueryOptions { get; set; }


        public VendorsListViewModel()
            : base()
        {
            Items = new List<VendorsListViewModel>();
            QueryOptions = new QueryOptions();
        }
        public VendorsListViewModel(UserSession userSession)
            : base(userSession)
        {
            Items = new List<VendorsListViewModel>();
            QueryOptions = new QueryOptions();
        }
        public VendorsListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Items = new List<VendorsListViewModel>();
            QueryOptions = new QueryOptions();
        }
    }
}