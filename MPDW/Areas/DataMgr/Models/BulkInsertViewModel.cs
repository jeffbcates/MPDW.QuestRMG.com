using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MasterPricing.DataMgr.Models
{
    public class BulkInsertViewModel : DataMgrBaseViewModel
    {
        public int TablesetId { get; set; }
        public int FilterId { get; set; }
        public List<BulkInsertRowDataViewModel> Rows { get; set; }


        public BulkInsertViewModel()
            : base()
        {
            Rows = new List<BulkInsertRowDataViewModel>();
        }
        public BulkInsertViewModel(UserSession userSession)
            : base(userSession)
        {
            Rows = new List<BulkInsertRowDataViewModel>();
        }
        public BulkInsertViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Rows = new List<BulkInsertRowDataViewModel>();
        }
    }
}