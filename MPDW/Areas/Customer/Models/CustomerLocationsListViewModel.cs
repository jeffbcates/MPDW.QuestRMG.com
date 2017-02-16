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
    public class CustomerLocationsListViewModel : CustomerBaseViewModel
    {
        public List<CustomerLocationsListViewModel> Items { get; set; }
        public QueryOptions QueryOptions { get; set; }


        public CustomerLocationsListViewModel(UserSession userSession)
            : base(userSession)
        {
            Items = new List<CustomerLocationsListViewModel>();
            QueryOptions = new QueryOptions();
        }
        public CustomerLocationsListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Items = new List<CustomerLocationsListViewModel>();
            QueryOptions = new QueryOptions();
        }
    }
}