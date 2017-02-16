using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.FMS
{
    public class RequisitionAuxillarySkill
    {
        public int Id { get; set; }
        public int RequisitionId { get; set; }
        public string Skill { get; set; }
    }
}
