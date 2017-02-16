using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.MasterPricing
{
    public class FilterViewNameIdentifier
    {
        public FilterId FilterId { get; set; }
        public string Schema { get; set; }
        public string Name { get; set; }

        public FilterViewNameIdentifier()
        {

        }
        public FilterViewNameIdentifier(FilterId filterId, string schema, string name)
        {
            FilterId = filterId;
            Schema = schema;
            Name = name;
        }
    }
}
