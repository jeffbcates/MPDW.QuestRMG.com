using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Status;


namespace Avid.Functional.FMS.Observer
{
    public class ListenerAlpha : Listener
    {       
        public override questStatus Notify()
        {
            return (new questStatus(Severity.Warning, "Not implemented"));
        }


        public ListenerAlpha(EventDispatcher eventDispatcher)
        {
            this.eventDispatcher = eventDispatcher;
        }
    }
}
