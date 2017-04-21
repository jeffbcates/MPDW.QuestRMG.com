using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Functional.MasterPricing
{
    public class FilterColumn
    {
        public int Id { get; set; }
        public int FilterId { get; set; }
        public int FilterEntityTypeId { get; set; }
        public int FilterEntityId { get; set; }
        public string Name { get; set; }

        public EntityType ParentEntityType { get; set; }

        public TablesetColumn TablesetColumn { get; set; }
        public int TablesetEntityId { get; set; }

        public FilterColumn()
        {
            TablesetColumn = new TablesetColumn();
            ParentEntityType = new EntityType();
        }
    }
}
