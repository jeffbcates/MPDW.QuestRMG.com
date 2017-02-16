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
    public class CustomerServiceRequestsListViewModel : CustomerBaseViewModel
    {
        public List<CustomerServiceRequestsListViewModel> Items { get; set; }
        public QueryOptions QueryOptions { get; set; }


        public CustomerServiceRequestsListViewModel(UserSession userSession)
            : base(userSession)
        {
            Items = new List<CustomerServiceRequestsListViewModel>();
            QueryOptions = new QueryOptions();
        }
        public CustomerServiceRequestsListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Items = new List<CustomerServiceRequestsListViewModel>();
            QueryOptions = new QueryOptions();
        }
    }
}