using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Status;


namespace Avid.Functional.FMS.Observer
{
    public abstract class Listener
    {
        protected EventDispatcher eventDispatcher = null;
        public abstract questStatus Notify();
    }
}
