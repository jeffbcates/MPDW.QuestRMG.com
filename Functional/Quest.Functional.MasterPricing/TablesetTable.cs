using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Functional.MasterPricing
{
    public class TablesetTable
    {
        public int Id { get; set; }
        public int TablesetId { get; set; }
        public string Schema { get; set; }
        public string Name { get; set; }
        public List<TablesetColumn> TablesetColumnList { get; set; }
        public Table Table { get; set; }


        public TablesetTable()
        {
            TablesetColumnList = new List<TablesetColumn>();
            Table = new Table();
        }
    }
}
