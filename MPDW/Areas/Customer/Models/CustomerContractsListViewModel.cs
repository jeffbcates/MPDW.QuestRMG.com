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
    public class CustomerContractsListViewModel : CustomerBaseViewModel
    {
        public List<CustomerContractsListViewModel> Items { get; set; }
        public QueryOptions QueryOptions { get; set; }


        public CustomerContractsListViewModel(UserSession userSession)
            : base(userSession)
        {
            Items = new List<CustomerContractsListViewModel>();
            QueryOptions = new QueryOptions();
        }
        public CustomerContractsListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Items = new List<CustomerContractsListViewModel>();
            QueryOptions = new QueryOptions();
        }
    }
}