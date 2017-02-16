using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;
using Quest.Functional.ASM;


namespace Quest.Functional.FMS
{
    public class EmployerUser
    {
        public int Id { get; set; }
        public int EmployerId { get; set; }
        public int UserId { get; set; }
        public DateTime Created { get; set; }

        public Employer Employer { get; set; }
        public User User { get; set; }


        public EmployerUser()
        {
            Employer = new Employer();
            User = new User();
        }
    }
}
