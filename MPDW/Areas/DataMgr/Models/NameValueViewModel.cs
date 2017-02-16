using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MasterPricing.DataMgr.Models
{
    public class NameValueViewModel 
    {
        public string ColumnName { get; set; }
        public string ColumnValue { get; set; }
        public bool bNull { get; set; }
        public int LookupId { get; set; }
        public int TypeListId { get; set; }
    }
}