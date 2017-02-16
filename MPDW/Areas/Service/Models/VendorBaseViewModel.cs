using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MPDW.Service.Models
{
    public class ServiceBaseViewModel : BaseUserSessionViewModel
    {

        public ServiceBaseViewModel() { }
        public ServiceBaseViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public ServiceBaseViewModel(UserSession userSession, BaseViewModel baseViewModel)
            : base(userSession, baseViewModel)
        {
        }
    }
}