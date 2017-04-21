using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Functional.MasterPricing
{
    public class FilterTable
    {
        public int Id { get; set; }
        public int FilterId { get; set; }
        public string Schema { get; set; }
        public string Name { get; set; }
        public TablesetTable TablesetTable { get; set; }
        public List<FilterColumn> FilterColumnList { get; set; }

        public string Acronym { get; set; }


        public FilterTable()
        {
            TablesetTable = new TablesetTable();
            FilterColumnList = new List<FilterColumn>();
        }
    }
}
