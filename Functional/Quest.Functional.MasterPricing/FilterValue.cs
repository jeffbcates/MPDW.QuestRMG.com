using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Functional.MasterPricing
{
    public class FilterValue
    {
        public int Id { get; set; }
        public int FilterOperationId { get; set; }
        public string Value { get; set; }


        public FilterValue()
        {
        }
    }
}
