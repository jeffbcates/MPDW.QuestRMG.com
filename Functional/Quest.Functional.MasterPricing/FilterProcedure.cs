using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Functional.MasterPricing
{
    public class FilterProcedure
    {
        public int Id { get; set; }
        public int FilterId { get; set; }
        public string Action { get; set; }
        public string Name { get; set; }
        public List<FilterProcedureParameter> ParameterList { get; set; }


        public FilterProcedure()
        {
            ParameterList = new List<FilterProcedureParameter>();
        }
    }
}
