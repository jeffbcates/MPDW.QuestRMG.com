using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Status;


namespace Avid.Functional.FMS.Factory
{
    public interface IFactoryInterface
    {
        questStatus Operation1();
        questStatus Operation2();
        questStatus Operation3();
    }
}
