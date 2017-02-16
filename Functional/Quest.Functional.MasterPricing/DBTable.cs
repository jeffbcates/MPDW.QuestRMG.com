using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.MasterPricing
{
    public class DBTable
    {
        public int Id { get; set; }
        public string Schema { get; set; }
        public string Name { get; set; }
        public List<DBColumn> Columns { get; set; }


        public DBTable()
        {
            Id = BaseId.INVALID_ID;
            Columns = new List<DBColumn>();
        }
    }
}
