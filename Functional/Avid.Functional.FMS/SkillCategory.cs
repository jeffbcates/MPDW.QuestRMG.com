using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.FMS
{
    public class SkillCategory
    {
        public int Id { get; set; }
        public int SectorId { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }

        public List<SkillCategory> Children { get; set; }

        public SkillCategory()
        {
            Children = new List<SkillCategory>();
        }
    }
}
