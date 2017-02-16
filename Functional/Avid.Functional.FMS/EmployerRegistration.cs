using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Functional.ASM;


namespace Quest.Functional.FMS
{
    public class EmployerRegistration
    {
        public Employer Employer { get; set; }
        public User User { get; set; }


        public EmployerRegistration()
        {
            Employer = new Employer();
            User = new User();
        }
    }
}
