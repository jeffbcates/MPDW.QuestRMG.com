using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.MasterPricing
{
    public class FilterColumnTablesetColumnId
    {
        public EntityTypeId EntityTypeId { get; set; }
        public EntityId EntityId { get; set; }
        public string Name { get; set; }

        public FilterColumnTablesetColumnId()
        {

        }
        public FilterColumnTablesetColumnId(EntityTypeId entityTypeId, EntityId entityId, string name)
        {
            EntityTypeId = entityTypeId;
            EntityId = entityId;
            Name = name;
        }
    }
}
