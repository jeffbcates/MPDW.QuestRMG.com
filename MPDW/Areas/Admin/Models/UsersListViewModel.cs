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
    public class UsersListViewModel : AdminBaseListViewModel
    {
        public List<UserLineItemViewModel> Items { get; set; }


        public UsersListViewModel()
            : base()
        {
            Items = new List<UserLineItemViewModel>();
        }
        public UsersListViewModel(UserSession userSession)
            : base(userSession)
        {
            Items = new List<UserLineItemViewModel>();
        }
        public UsersListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Items = new List<UserLineItemViewModel>();
        }
    }
}