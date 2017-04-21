using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.MasterPricing
{
    public class FilterViewTablesetViewId
    {
        public TablesetId TablesetId { get; set; }
        public string Schema { get; set; }
        public string Name { get; set; }

        public FilterViewTablesetViewId()
        {

        }
        public FilterViewTablesetViewId(TablesetId tablesetId, string schema, string name)
        {
            TablesetId = tablesetId;
            Schema = schema;
            Name = name;
        }
    }
}
