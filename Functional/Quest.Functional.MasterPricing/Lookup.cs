using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Functional.MasterPricing
{
    public class Lookup
    {
        public int Id { get; set; }
        public int DatabaseId { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string Summary { get; set; }
        public string SQL { get; set; }
        public string KeyField { get; set; }
        public string TextFields { get; set; }
        public string Arguments { get; set; }
        public string Database { get; set; }
    }
}
