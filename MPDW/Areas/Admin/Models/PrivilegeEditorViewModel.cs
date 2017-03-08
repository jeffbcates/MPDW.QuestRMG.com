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
    public class PrivilegeEditorViewModel : AdminBaseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        public PrivilegeEditorViewModel()
            : base()
        {
        }
        public PrivilegeEditorViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public PrivilegeEditorViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
        }
    }
}