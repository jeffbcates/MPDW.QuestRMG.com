using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Functional.MasterPricing
{
    public class TypeList
    {
        public int Id { get; set; }
        public int DatabaseId { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string SQL { get; set; }
        public string Database { get; set; }
    }
}
