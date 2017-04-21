using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Functional.MasterPricing;

namespace Quest.Functional.MasterPricing
{
    public class FilterFolder
    {
        public int Id { get; set; }
        public int? FolderId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
        public List<FilterFolder> Folders { get; set; }
        public List<Filter> Filters { get; set; }


        public FilterFolder()
        {
            Folders = new List<FilterFolder>();
            Filters = new List<Filter>();
        }
    }
}
