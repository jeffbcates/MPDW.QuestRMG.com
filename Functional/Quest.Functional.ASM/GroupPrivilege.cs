using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.ASM
{
    public class GroupPrivilege
    {
        public int Id { get; set; }
        public Privilege Privilege { get; set; }
        public Group Group { get; set; }
        public DateTime Created { get; set; }


        public GroupPrivilege()
        {
            Privilege = new Privilege();
            Group = new Group();
        }
    }
}
