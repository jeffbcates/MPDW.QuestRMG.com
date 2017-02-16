using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Functional.MasterPricing
{
    public class BulkUpdateColumnValue
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool bNull { get; set; }
        public int LookupId { get; set; }
        public int TypeListId { get; set; }

        public FilterColumn FilterColumn { get; set; }
    }
}
