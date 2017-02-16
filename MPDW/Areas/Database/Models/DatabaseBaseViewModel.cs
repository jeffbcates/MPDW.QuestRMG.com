using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MasterPricing.Database.Models
{
    public class DatabaseBaseViewModel : BaseUserSessionViewModel
    {
        public DatabaseBaseViewModel() { }
        public DatabaseBaseViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public DatabaseBaseViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
        }
    }
}