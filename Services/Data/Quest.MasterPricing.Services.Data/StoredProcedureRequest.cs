using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Functional.MasterPricing;


namespace Quest.MasterPricing.Services.Data
{
    public class StoredProcedureRequest
    {
        public Quest.Functional.MasterPricing.Database Database { get; set; }
        public StoredProcedure StoredProcedure { get; set; }


        public StoredProcedureRequest()
        {
            Database = new Quest.Functional.MasterPricing.Database();
            StoredProcedure = new StoredProcedure();
        }
    }
}
