using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avid.Functional.FMS.Command
{
    public class Command
    {
        private string _type = null;
        private object _commandObject = null;

        public string Type
        {
            get
            {
                return (_type);
            }
        }
        public Object CommandObject
        {
            get
            {
                return (_commandObject);
            }
        }

        public Command(string type, object commandObject)
        {
            _type = type;
            _commandObject = commandObject;
        }
    }
}
