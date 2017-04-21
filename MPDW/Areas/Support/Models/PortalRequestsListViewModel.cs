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
    public class PortalRequestsListViewModel : SupportBaseListViewModel
    {
        public List<PortalRequestLineItemViewModel> Items { get; set; }


        public PortalRequestsListViewModel()
            : base()
        {
            Items = new List<PortalRequestLineItemViewModel>();
        }
        public PortalRequestsListViewModel(UserSession userSession)
            : base(userSession)
        {
            Items = new List<PortalRequestLineItemViewModel>();
        }
        public PortalRequestsListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Items = new List<PortalRequestLineItemViewModel>();
        }
    }
}