using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.ASM
{
    public class GroupUser
    {
        public int Id { get; set; }
        public Group Group { get; set; }
        public User User { get; set; }
        public DateTime Created { get; set; }


        public GroupUser()
        {
            Group = new Group();
            User = new User();
        }
    }
}
