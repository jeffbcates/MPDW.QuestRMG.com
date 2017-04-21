using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Quest.Functional.MasterPricing
{
    public class EntityType
    {
        public int Id { get; set; }
        public const int Table = 1;
        public const int View = 2;

        public string type { get; set; }
        public string Schema { get; set; }
        public string Name { get; set; }
    }
}
