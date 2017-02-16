using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Functional.MasterPricing
{
    public class TablesetDataManagement
    {
        public TablesetConfiguration TablesetConfiguration { get; set; }
        public List<Filter> FilterList { get; set; }


        public TablesetDataManagement()
        {
            TablesetConfiguration = new TablesetConfiguration();
            FilterList = new List<Filter>();
        }
    }
}
