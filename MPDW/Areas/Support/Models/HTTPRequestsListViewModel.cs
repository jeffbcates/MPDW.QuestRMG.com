using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.Util.Data;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MPDW.Support.Models
{
    public class HTTPRequestsListViewModel : SupportBaseListViewModel
    {
        public List<HTTPRequestLineItemViewModel> Items { get; set; }


        public HTTPRequestsListViewModel()
            : base()
        {
            Items = new List<HTTPRequestLineItemViewModel>();
        }
        public HTTPRequestsListViewModel(UserSession userSession)
            : base(userSession)
        {
            Items = new List<HTTPRequestLineItemViewModel>();
        }
        public HTTPRequestsListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Items = new List<HTTPRequestLineItemViewModel>();
        }
    }
}