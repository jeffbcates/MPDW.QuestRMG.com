using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Functional.MasterPricing
{
    public class DatabaseEntities
    {
        public Database Database { get; set; }
        public List<Table> TableList { get; set; }
        public List<View> ViewList { get; set; }
        public List<StoredProcedure> StoredProcedureList { get; set; }


        public DatabaseEntities()
        {
            Database = new Database();
            TableList = new List<Table>();
            ViewList = new List<View>();
            StoredProcedureList = new List<StoredProcedure>();
        }
    }
}
