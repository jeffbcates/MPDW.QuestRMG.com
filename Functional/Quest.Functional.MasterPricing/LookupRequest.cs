using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Functional.MasterPricing
{
    public class LookupRequest
    {
        public LookupId LookupId { get; set; }
        public FilterItemId FilterItemId { get; set; }


        public LookupRequest()
        {
            LookupId = new LookupId();
            FilterItemId = new FilterItemId();
        }
    }
}
