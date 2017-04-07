﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.Functional.MasterPricing;
using Quest.MPDW.Models.List;


namespace Quest.MPDW.Support.Models
{
    public class SupportBaseListViewModel : SupportBaseViewModel
    {
        public QueryOptionsViewModel QueryOptions { get; set; }
        public QueryResponseViewModel QueryResponse { get; set; }

        public SupportBaseListViewModel() 
        {
            QueryOptions = new QueryOptionsViewModel();
            QueryResponse = new QueryResponseViewModel();
        }
        public SupportBaseListViewModel(UserSession userSession)
            : base(userSession)
        {
            QueryOptions = new QueryOptionsViewModel();
            QueryResponse = new QueryResponseViewModel();
        }
        public SupportBaseListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            QueryOptions = new QueryOptionsViewModel();
            QueryResponse = new QueryResponseViewModel();
        }
    }
}