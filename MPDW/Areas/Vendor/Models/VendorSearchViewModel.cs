using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MPDW.Vendor.Models
{
    public class VendorSearchViewModel : BaseUserSessionViewModel
    {
        public VendorSearchViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public VendorSearchViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
        }
    }
}