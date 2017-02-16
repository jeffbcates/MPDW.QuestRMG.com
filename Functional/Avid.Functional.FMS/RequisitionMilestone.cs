using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.FMS
{
    public class RequisitionMilestone
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int RequisitionId { get; set; }
        public int? ParentMilestoneId { get; set; }
        public int PercentComplete { get; set; }
        public string Label { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
