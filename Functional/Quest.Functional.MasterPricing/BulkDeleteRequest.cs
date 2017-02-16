using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Quest.Functional.MasterPricing
{
    public class BulkDeleteRequest
    {
        public Filter Filter { get; set; }
        public string SQL { get; set; }


        public BulkDeleteRequest()
        {
            Filter = new Filter();
        }
    }
}
