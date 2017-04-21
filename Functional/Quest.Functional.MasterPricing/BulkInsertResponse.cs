using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Functional.MasterPricing
{
    public class BulkInsertResponse
    {
        public BulkInsertRequest BulkInsertRequest { get; set; }
        public List<BulkInsertRow> ValidationErrors { get; set; }
        
        // TODO: INVALID FIELDS.

        public BulkInsertResponse()
        {
            BulkInsertRequest = new BulkInsertRequest();
            ValidationErrors = new List<BulkInsertRow>();
        }
    }
}
