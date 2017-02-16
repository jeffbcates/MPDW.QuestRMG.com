using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Quest.Functional.MasterPricing
{
    public class FilterEntityType
    {
        public int Id { get; set; }
        public const int Table = 1;
        public const int Column = 2;
        public const int View = 3;
        public const int StoredProcedure = 4;
        public const int Function = 5;

        public string Name { get; set; }
    }
}
