using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Functional.MasterPricing
{
    public class TablesetConfiguration
    {
        public Tableset Tableset { get; set; }
        public Database Database { get; set; }

        public List<TablesetTable> TablesetTables { get; set; }
        public List<TablesetView> TablesetViews { get; set; }

        public List<Table> DBTableList { get; set; }
        public List<View> DBViewList { get; set; }


        public TablesetConfiguration()
        {
            Tableset = new Tableset();
            Database = new Database();

            TablesetTables = new List<TablesetTable>();
            TablesetViews = new List<TablesetView>();

            DBTableList = new List<Table>();
            DBViewList = new List<View>();
        }
    }
}
