using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;
using Quest.Functional.ASM;


namespace Quest.Functional.FMS
{
    public class EmployerRegistrationId : BaseId
    {
        public EmployerId EmployerId { get; set; }
        public UserId UserId { get; set; }

        public EmployerRegistrationId()
            : base()
        {

        }
    }
}
