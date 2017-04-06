using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.Logging
{
    public class BulkInsertLogId : BaseId
    {
        public BulkInsertLogId()
            : base()
        {

        }
        public BulkInsertLogId(int Id)
            : base(Id)
        {

        }
    }
}
