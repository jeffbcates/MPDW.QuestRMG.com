using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Status;


namespace Avid.Functional.FMS.Observer
{
    public class EventDispatcher
    {
        private List<Listener> _listenerList = null;
        private int _state = -1;

        public int State
        {
            get
            {
                return (this._state);
            }
        }
        public questStatus SetState(int state)
        {
            questStatus status = null;

            this._state = state;
            status = Notify();
            if (! questStatusDef.IsSuccess(status))
            {
                return (status);
            }
            return (new questStatus(Severity.Success));
        }

        public questStatus Notify()
        {
            foreach (Listener listener in _listenerList)
            {
                listener.Notify();
            }
            return (new questStatus(Severity.Success));
        }

        public EventDispatcher()
        {
            _listenerList = new List<Listener>();
        }
    }
}
