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
    public class UserGroupsViewModel : AdminBaseViewModel
    {
        public int Id { get; set; }
        public UserEditorViewModel User { get; set; }
        public List<GroupViewModel> Groups { get; set; }
        public List<GroupViewModel> UserGroups { get; set; }


        public UserGroupsViewModel()
            : base()
        {
            User = new UserEditorViewModel();
            Groups = new List<GroupViewModel>();
            UserGroups = new List<GroupViewModel>();
        }
        public UserGroupsViewModel(UserSession userSession)
            : base(userSession)
        {
            User = new UserEditorViewModel();
            Groups = new List<GroupViewModel>();
            UserGroups = new List<GroupViewModel>();
        }
        public UserGroupsViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            User = new UserEditorViewModel();
            Groups = new List<GroupViewModel>();
            UserGroups = new List<GroupViewModel>();
        }
    }
}