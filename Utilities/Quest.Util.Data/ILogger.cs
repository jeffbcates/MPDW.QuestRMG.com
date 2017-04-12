using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Status;


namespace Quest.Util.Data
{
    interface ILogger
    {
        questStatus LogException(System.Exception ex);
    }
}
