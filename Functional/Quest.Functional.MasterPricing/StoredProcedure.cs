using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Functional.MasterPricing
{
    public class StoredProcedure
    {
        public int Id { get; set; }
        public int DatabaseId { get; set; }
        public string Name { get; set; }
        public List<StoredProcedureParameter> ParameterList { get; set; }


        public StoredProcedure()
        {
            ParameterList = new List<StoredProcedureParameter>();
        }
    }
}
