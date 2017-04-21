using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Quest.MasterPricing.DataMgr.Models
{
    [Serializable]
    public class ColumnValueViewModel
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }
    }
}