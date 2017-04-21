using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Status;

namespace Avid.Functional.FMS.Factory
{
    public class OperatorBeta : IFactoryInterface
    {
        questStatus IFactoryInterface.Operation1()
        {
            return (new questStatus(Severity.Warning, "Not implemented"));
        }
        questStatus IFactoryInterface.Operation2()
        {
            return (new questStatus(Severity.Warning, "Not implemented"));
        }
        questStatus IFactoryInterface.Operation3()
        {
            return (new questStatus(Severity.Warning, "Not implemented"));
        }
    }
}
