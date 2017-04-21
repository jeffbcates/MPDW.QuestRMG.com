using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.MasterPricing
{
    public class FilterTableTablesetTableId
    {
        public TablesetId TablesetId { get; set; }
        public string Schema { get; set; }
        public string Name { get; set; }

        public FilterTableTablesetTableId()
        {

        }
        public FilterTableTablesetTableId(TablesetId tablesetId, string schema, string name)
        {
            TablesetId = tablesetId;
            Schema = schema;
            Name = name;
        }
    }
}
