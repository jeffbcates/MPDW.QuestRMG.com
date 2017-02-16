using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.FMS
{
    public class ProfileId : BaseId
    {
        public ProfileTypeId ProfileTypeId { get; set; }

        public ProfileId()
            : base()
        {
            ProfileTypeId = new ProfileTypeId();
        }
        public ProfileId(ProfileTypeId profileTypeId, int Id)
            : base(Id)
        {
            ProfileTypeId = new ProfileTypeId(profileTypeId.Id);
        }
    }
}
