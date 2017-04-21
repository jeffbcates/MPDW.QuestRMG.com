using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Functional.MasterPricing
{
    public class Tableset
    {
        public int Id { get; set; }
        public int DatabaseId { get; set; }
        public bool bEnabled { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Database { get; set; }
        public DateTime? LastRefresh { get; set; }
    }
}
