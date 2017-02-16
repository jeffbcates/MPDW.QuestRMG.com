using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Functional.MasterPricing
{
    public class TablesetColumn
    {
        public int Id { get; set; }
        public int EntityTypeId { get; set; }
        public int TableSetEntityId { get; set; }
        public string Name { get; set; }
        public Column Column { get; set; }


        public TablesetColumn()
        {
            Column = new Column();
        }
    }
}
