using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.FMS
{
    public class ProfileTypeId : BaseId
    {
        public static int Employer = 1;
        public static int Framework = 2;
        public static int SolutionShop = 3;

        public ProfileTypeId()
            : base()
        {

        }
        public ProfileTypeId(int Id)
            : base(Id)
        {

        }
    }
}
