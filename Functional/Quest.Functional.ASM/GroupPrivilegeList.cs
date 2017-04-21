using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.ASM
{
    public class GroupPrivilegeList
    {
        public int Id { get; set; }
        public Group Group { get; set; }
        public List<Privilege> PrivilegeList { get; set; }
        public DateTime Created { get; set; }

        public GroupPrivilegeList()
        {
            Group = new Group();
            PrivilegeList = new List<Privilege>();
        }
    }
}
