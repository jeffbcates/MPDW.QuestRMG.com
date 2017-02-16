using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Functional.MasterPricing
{
    public class View
    {
        public int Id { get; set; }
        public int DatabaseId { get; set; }
        public string Schema { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public List<Column> ColumnList { get; set; }


        public View()
        {
            ColumnList = new List<Column>();
        }
    }
}
