using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.ASM
{
    public class PrivilegeUserList
    {
        public int Id { get; set; }
        public Privilege Privilege { get; set; }
        public List<User> UserList { get; set; }
        public DateTime Created { get; set; }


        public PrivilegeUserList()
        {
            Privilege = new Privilege();
            UserList = new List<User>();
        }
    }
}
