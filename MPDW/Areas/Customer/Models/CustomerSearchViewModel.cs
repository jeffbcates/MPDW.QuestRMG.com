using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MPDW.Customer.Models
{
    public class CustomerSearchViewModel : BaseUserSessionViewModel
    {
        public CustomerSearchViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public CustomerSearchViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
        }
    }
}