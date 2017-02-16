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
    public class CustomersListViewModel : CustomerBaseViewModel
    {
        public List<CustomersListViewModel> Items { get; set; }
        public QueryOptions QueryOptions { get; set; }


        public CustomersListViewModel()
            : base()
        {
            Items = new List<CustomersListViewModel>();
            QueryOptions = new QueryOptions();
        }
        public CustomersListViewModel(UserSession userSession)
            : base(userSession)
        {
            Items = new List<CustomersListViewModel>();
            QueryOptions = new QueryOptions();
        }
        public CustomersListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Items = new List<CustomersListViewModel>();
            QueryOptions = new QueryOptions();
        }
    }
}