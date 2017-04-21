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
    public class GroupsListViewModel : AdminBaseListViewModel
    {
        public List<GroupLineItemViewModel> Items { get; set; }


        public GroupsListViewModel()
            : base()
        {
            Items = new List<GroupLineItemViewModel>();
        }
        public GroupsListViewModel(UserSession userSession)
            : base(userSession)
        {
            Items = new List<GroupLineItemViewModel>();
        }
        public GroupsListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Items = new List<GroupLineItemViewModel>();
        }
    }
}