using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Functional.MasterPricing
{
    public class BulkInsertRow
    {
        public List<BulkInsertColumnValue> Columns { get; set; }


        public BulkInsertRow()
        {
            Columns = new List<BulkInsertColumnValue>();
        }
    }
}
