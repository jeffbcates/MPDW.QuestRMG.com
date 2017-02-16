using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MPDW.Vendor.Models
{
    public class VendorBaseViewModel : BaseUserSessionViewModel
    {

        public VendorBaseViewModel() { }
        public VendorBaseViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public VendorBaseViewModel(UserSession userSession, BaseViewModel baseViewModel)
            : base(userSession, baseViewModel)
        {
        }
    }
}