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
    public class UserPrivilegesViewModel : AdminBaseViewModel
    {
        public int Id { get; set; }
        public UserEditorViewModel User { get; set; }
        public List<BootstrapTreenodeViewModel> Privileges { get; set; }
        public List<BootstrapTreenodeViewModel> UserPrivileges { get; set; }


        public UserPrivilegesViewModel()
            : base()
        {
            User = new UserEditorViewModel();
            Privileges = new List<BootstrapTreenodeViewModel>();
            UserPrivileges = new List<BootstrapTreenodeViewModel>();
        }
        public UserPrivilegesViewModel(UserSession userSession)
            : base(userSession)
        {
            User = new UserEditorViewModel();
            Privileges = new List<BootstrapTreenodeViewModel>();
            UserPrivileges = new List<BootstrapTreenodeViewModel>();
        }
        public UserPrivilegesViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            User = new UserEditorViewModel();
            Privileges = new List<BootstrapTreenodeViewModel>();
            UserPrivileges = new List<BootstrapTreenodeViewModel>();
        }
    }
}