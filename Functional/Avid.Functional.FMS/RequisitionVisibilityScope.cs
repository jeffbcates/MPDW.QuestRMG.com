using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.FMS
{
    public class RequisitionVisibilityScope
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool bActive { get; set; }
        public DateTime Created { get; set; }
    }
}
