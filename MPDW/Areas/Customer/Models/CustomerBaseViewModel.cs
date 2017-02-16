using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MPDW.Customer.Models
{
    public class CustomerBaseViewModel : BaseUserSessionViewModel
    {

        public CustomerBaseViewModel() { }
        public CustomerBaseViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public CustomerBaseViewModel(UserSession userSession, BaseViewModel baseViewModel)
            : base(userSession, baseViewModel)
        {
        }
    }
}