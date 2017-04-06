using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.ASM
{
    public class PrivilegeUser
    {
        public int Id { get; set; }
        public Privilege Privilege { get; set; }
        public User User { get; set; }
        public DateTime Created { get; set; }


        public PrivilegeUser()
        {
            Privilege = new Privilege();
            User = new User();
        }
    }
}
