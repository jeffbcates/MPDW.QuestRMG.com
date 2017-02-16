using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.FMS
{
    public class FrameworkSkill
    {
        public int Id { get; set; }
        public Framework Framework { get; set; }
        public List<Skill> SkillList { get; set; }

        public FrameworkSkill()
        {
            Framework = new Framework();
            SkillList = new List<Skill>();
        }
    }
}
