using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.FMS
{
    public class RequisitionWatchList
    {
        public int Id { get; set; }
        public int ProfileTypeId { get; set; }
        public int ProfileId { get; set; }
        public int RequisitionId { get; set; }
        public string Notes { get; set; }
        public DateTime Created { get; set; }
    }
}
