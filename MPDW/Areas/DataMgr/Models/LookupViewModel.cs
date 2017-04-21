using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Quest.MasterPricing.DataMgr.Models
{
    public class LookupViewModel : DataMgrBaseViewModel
    {
        public int Id { get; set; }
        public int FilterItemId { get; set; }
        public List<string> Arguments { get; set; }
    }
}