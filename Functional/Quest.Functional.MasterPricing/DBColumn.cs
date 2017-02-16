using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Functional.MasterPricing
{
    public class DBColumn
    {
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        public string DataType { get; set; }
        public string DataTypeName { get; set; }
        public int ColumnSize { get; set; }
        public bool bIsIdentity { get; set; }
        public bool bAllowDbNull { get; set; }
    }
}
