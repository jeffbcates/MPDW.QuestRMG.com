using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Util.Data
{
    public class SearchField
    {
        public string Name { get; set; }
        public SearchOperation SearchOperation { get; set; }
        public Type Type { get; set; }
        public string Value { get; set; }


        public SearchField()
        {
            SearchOperation = SearchOperation.Equal;
        }
    }
}
