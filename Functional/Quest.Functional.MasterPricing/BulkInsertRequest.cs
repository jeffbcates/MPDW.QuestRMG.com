using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Quest.Functional.MasterPricing
{
    public class BulkInsertRequest
    {
        public int TablesetId { get; set; }
        public int FilterId { get; set; }
        public string StoredProcedure { get; set; }
        public List<BulkInsertRow> Rows { get; set; }

        public Filter Filter { get; set; }


        // TODO:...
        public string SQL { get; set; }


        public BulkInsertRequest()
        {
            Rows = new List<BulkInsertRow>();
        }
    }
}
