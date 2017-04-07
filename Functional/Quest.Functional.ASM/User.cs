using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.ASM
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password {get; set; }
        public bool bEnabled { get; set; }
        public bool bActive { get; set; }
        public bool bLogSession { get; set; }
        public DateTime Created { get; set; }
    }
}
