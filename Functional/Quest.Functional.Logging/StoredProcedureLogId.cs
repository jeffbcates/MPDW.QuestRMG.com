using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.Logging
{
    public class StoredProcedureLogId : BaseId
    {
        public StoredProcedureLogId()
            : base()
        {

        }
        public StoredProcedureLogId(int Id)
            : base(Id)
        {

        }
    }
}
