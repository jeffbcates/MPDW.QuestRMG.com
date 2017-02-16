using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Quest.Functional.MasterPricing
{
    public class BulkUpdateRequest
    {
        public List<BulkUpdateColumnValue> Columns { get; set; }
        public int FilterId { get; set;  }
        public Filter Filter { get; set; }
        public string SQL { get; set; }


        public BulkUpdateRequest()
        {
            Columns = new List<BulkUpdateColumnValue>();
            Filter = new Filter();
        }
    }
}
