using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MPDW.Search.Models
{
    public class EmployerUsersSearchViewModel : BaseUserSessionViewModel
    {
        public EmployerUsersSearchViewModel() { }
        public EmployerUsersSearchViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public EmployerUsersSearchViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
        }
    }
}