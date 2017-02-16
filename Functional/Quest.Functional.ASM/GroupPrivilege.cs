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
        public int GroupId { get; set; }
        public int PrivilegeId { get; set; }
        public DateTime Created { get; set; }
    }
}
