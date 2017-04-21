using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MasterPricing.DataMgr.Models
{
    public class BulkDeleteViewModel : DataMgrBaseViewModel
    {
        public List<NameValueViewModel> ColumnData { get; set; }


        public BulkDeleteViewModel()
            : base()
        {
            ColumnData = new List<NameValueViewModel>();
        }
        public BulkDeleteViewModel(UserSession userSession)
            : base(userSession)
        {
            ColumnData = new List<NameValueViewModel>();
        }
        public BulkDeleteViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            ColumnData = new List<NameValueViewModel>();
        }
    }
}