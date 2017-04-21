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
    public class FilterEntityViewModel
    {
        public int Id { get; set; }
        public string type { get; set; }
        public string Name { get; set; }
        public string Schema { get; set; }

        public Column  Column { get; set; }


        public FilterEntityViewModel()
        {
            Column = new Column();
        }
    }
}