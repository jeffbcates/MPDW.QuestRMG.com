using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MasterPricing.DataMgr.Models
{
    public class BulkInsertRowDataViewModel
    {
        public List<NameValueViewModel> Columns { get; set; }


        public BulkInsertRowDataViewModel()
        {
            Columns = new List<NameValueViewModel>();
        }
    }
}