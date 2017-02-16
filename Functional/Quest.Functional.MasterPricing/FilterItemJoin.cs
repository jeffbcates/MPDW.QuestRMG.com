using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Functional.MasterPricing
{
    public class FilterItemJoin
    {
        public int Id { get; set; }
        public int FilterItemId { get; set; }
        public string JoinType { get; set; }
        public string Identifier { get; set; }
        public string SourceSchema { get; set; }
        public string SourceEntityName { get; set; }
        public string SourceColumnName { get; set; }
        public int TargetEntityTypeId { get; set; }
        public string TargetSchema { get; set; }
        public string TargetEntityName { get; set; }
        public string TargetColumnName { get; set; }

        public int ColumnId { get; set; }


        public FilterItemJoin()
        {
        }
    }
}
