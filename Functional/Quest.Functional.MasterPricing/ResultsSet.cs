using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Functional.MasterPricing
{
    public class ResultsSet
    {
        public FilterId FilterId { get; set; }
        public Dictionary<string, Column> ResultColumns = null;
        public List<dynamic> Data = null;


        public ResultsSet()
        {
            ResultColumns = new Dictionary<string, Column>();
            Data = new List<dynamic>();
        }
        public ResultsSet(FilterId filterId)
        {
            FilterId = filterId;
            ResultColumns = new Dictionary<string, Column>();
            Data = new List<dynamic>();
        }
    }
}
