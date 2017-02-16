using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Quest.Functional.MasterPricing
{
    public class RunFilterRequest
    {
        public FilterId FilterId { get; set; }
        public string RowLimit { get; set; }
        public string ColLimit { get; set; }
        public string PageNumber { get; set; }
        public string PageSize { get; set; }


        public RunFilterRequest()
        {
            FilterId = new FilterId();
        }
    }
}
