using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Functional.MasterPricing
{
    public class Database
    {
        public int Id { get; set; }
        public string Server { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string ConnectionString { get; set; }
        public DateTime? LastRefresh { get; set; }
    }
}
