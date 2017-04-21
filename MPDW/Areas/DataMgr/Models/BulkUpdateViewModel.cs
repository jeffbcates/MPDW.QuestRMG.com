using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.Functional.MasterPricing;


namespace Quest.MasterPricing.DataMgr.Models
{
    public class BulkUpdateViewModel : DataMgrBaseViewModel
    {
        public int FilterId { get; set; }
        public Quest.Functional.MasterPricing.Filter Filter { get; set; }
        public List<NameValueViewModel> ColumnData { get; set; }


        public BulkUpdateViewModel()
            : base()
        {
            ColumnData = new List<NameValueViewModel>();
        }
        public BulkUpdateViewModel(UserSession userSession)
            : base(userSession)
        {
            ColumnData = new List<NameValueViewModel>();
        }
        public BulkUpdateViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            ColumnData = new List<NameValueViewModel>();
        }
    }
}