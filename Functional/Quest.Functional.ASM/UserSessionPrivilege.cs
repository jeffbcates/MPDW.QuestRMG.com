using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.ASM
{
    public class UserSessionPrivilege
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PrivilegeId { get; set; }
        public DateTime Created { get; set; }

        public User User { get; set; }
        public Privilege Privilege { get; set; }

        public UserSessionPrivilege()
        {
            User = new User();
            Privilege = new Privilege();
        }
    }
}
