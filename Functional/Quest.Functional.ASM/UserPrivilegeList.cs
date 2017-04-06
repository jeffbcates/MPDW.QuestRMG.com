using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.ASM
{
    public class UserPrivilegeList
    {
        public int Id { get; set; }
        public User User { get; set; }
        public List<Privilege> PrivilegeList { get; set; }
        public DateTime Created { get; set; }


        public UserPrivilegeList()
        {
            User = new User();
            PrivilegeList = new List<Privilege>();
        }
    }
}
