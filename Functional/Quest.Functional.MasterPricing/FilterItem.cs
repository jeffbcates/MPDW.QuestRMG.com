using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Functional.MasterPricing
{
    public class FilterItem
    {
        public int Id { get; set; }
        public int FilterId { get; set; }
        public int FilterEntityTypeId { get; set; }
        public int FilterEntityId { get; set; }
        public int? LookupId { get; set; }
        public int? TypeListId { get; set; }
        public List<FilterItemJoin> JoinList { get; set; }
        public List<FilterOperation> OperationList { get; set; }
        public Lookup Lookup { get; set; }
        public TypeList TypeList { get; set; }

        public string Identifier { get; set; }
        public string Label { get; set; }
        public string ParameterName { get; set; }
        public bool bHidden { get; set; }
        public bool bBulkUpdateValueRequired { get; set; }

        public int TablesetColumnId { get; set; }

        public TablesetColumn TablesetColumn { get; set; }
        public FilterColumn FilterColumn { get; set; }


        public FilterItem()
        {
            JoinList = new List<FilterItemJoin>();
            OperationList = new List<FilterOperation>();
            Lookup = new Lookup();
            TypeList = new TypeList();
            TablesetColumn = new TablesetColumn();
            FilterColumn = new FilterColumn();
        }
    }
}
