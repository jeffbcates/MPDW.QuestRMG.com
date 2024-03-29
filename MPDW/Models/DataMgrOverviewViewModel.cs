﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.Functional.MasterPricing;


namespace Quest.MasterPricing.Models
{
    public class DataMgrOverviewViewModel : BaseUserSessionViewModel
    {
        public DataMgrOverviewViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public DataMgrOverviewViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
        }
    }
}