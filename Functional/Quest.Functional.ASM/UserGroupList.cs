using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.ASM
{
    public class UserGroupList
    {
        public int Id { get; set; }
        public User User { get; set; }
        public List<Group> GroupList { get; set; }
        public DateTime Created { get; set; }

        public UserGroupList()
        {
            User = new User();
            GroupList = new List<Group>();
        }
    }
}
