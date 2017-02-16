using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.Util.Data;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MPDW.Customer.Models
{
    public class CustomerServicesListViewModel : CustomerBaseViewModel
    {
        public List<CustomerServicesListViewModel> Items { get; set; }
        public QueryOptions QueryOptions { get; set; }


        public CustomerServicesListViewModel(UserSession userSession)
            : base(userSession)
        {
            Items = new List<CustomerServicesListViewModel>();
            QueryOptions = new QueryOptions();
        }
        public CustomerServicesListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Items = new List<CustomerServicesListViewModel>();
            QueryOptions = new QueryOptions();
        }
    }
}