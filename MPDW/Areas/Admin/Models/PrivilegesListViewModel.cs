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
    public class PrivilegesListViewModel : AdminBaseListViewModel
    {
        public List<PrivilegeLineItemViewModel> Items { get; set; }


        public PrivilegesListViewModel()
            : base()
        {
            Items = new List<PrivilegeLineItemViewModel>();
        }
        public PrivilegesListViewModel(UserSession userSession)
            : base(userSession)
        {
            Items = new List<PrivilegeLineItemViewModel>();
        }
        public PrivilegesListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Items = new List<PrivilegeLineItemViewModel>();
        }
    }
}