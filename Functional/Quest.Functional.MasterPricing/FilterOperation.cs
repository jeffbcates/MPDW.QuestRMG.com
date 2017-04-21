using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Quest.Functional.MasterPricing
{
    public class FilterOperation
    {
        public int Id { get; set; }
        public int FilterItemId { get; set; }
        public int FilterOperatorId { get; set; }
        public List<FilterValue> ValueList { get; set; }


        public FilterOperation()
        {
            ValueList = new List<FilterValue>();
        }
    }
}
