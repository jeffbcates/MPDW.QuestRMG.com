using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.ASM
{
    public class PrivilegeGroupList
    {
        public int Id { get; set; }
        public Privilege Privilege { get; set; }
        public List<Group> GroupList { get; set; }
        public DateTime Created { get; set; }


        public PrivilegeGroupList()
        {
            Privilege = new Privilege();
            GroupList = new List<Group>();
        }
    }
}
