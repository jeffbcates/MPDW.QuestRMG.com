using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Quest.MasterPricing.DataMgr.Models
{
    public class LookupItemViewModel
    {
        public int Id { get; set; }
        public int DatabaseId { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string Summary { get; set; }
        public string Database { get; set; }
    }
}