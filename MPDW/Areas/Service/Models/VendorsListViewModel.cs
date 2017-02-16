using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.Util.Data;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MPDW.Service.Models
{
    public class ServicesListViewModel : ServiceBaseViewModel
    {
        public List<ServicesListViewModel> Items { get; set; }
        public QueryOptions QueryOptions { get; set; }


        public ServicesListViewModel()
            : base()
        {
            Items = new List<ServicesListViewModel>();
            QueryOptions = new QueryOptions();
        }
        public ServicesListViewModel(UserSession userSession)
            : base(userSession)
        {
            Items = new List<ServicesListViewModel>();
            QueryOptions = new QueryOptions();
        }
        public ServicesListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Items = new List<ServicesListViewModel>();
            QueryOptions = new QueryOptions();
        }
    }
}