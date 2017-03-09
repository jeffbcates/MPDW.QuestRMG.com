using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.Util.Data;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MPDW.Admin.Models
{
    public class SessionsListViewModel : AdminBaseListViewModel
    {
        public List<SessionLineItemViewModel> Items { get; set; }


        public SessionsListViewModel()
            : base()
        {
            Items = new List<SessionLineItemViewModel>();
        }
        public SessionsListViewModel(UserSession userSession)
            : base(userSession)
        {
            Items = new List<SessionLineItemViewModel>();
        }
        public SessionsListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Items = new List<SessionLineItemViewModel>();
        }
    }
}