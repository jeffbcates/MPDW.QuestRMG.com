using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Functional.MasterPricing
{
    public class DatabaseMetaInfo
    {
        public DatabaseId DatabaseId { get; set; }
        public Dictionary<DBTable, List<Column>> DBTableDictionary { get; set; }
        public Dictionary<DBView, List<Column>> DBViewDictionary { get; set; }
        public List<StoredProcedure> StoredProcedureList { get; set; }


        public DatabaseMetaInfo()
        {
            DatabaseId = new DatabaseId();
            DBTableDictionary = new Dictionary<DBTable, List<Column>>();
            DBViewDictionary = new Dictionary<DBView, List<Column>>();
            StoredProcedureList = new List<StoredProcedure>();
        }
    }
}
