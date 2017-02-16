using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Util.Status
{
    public class questStatus
    {
        public uint Status;
        public Severity Severity;
        public string Module;
        public string Method;
        public string Message;


        public questStatus(Severity severity)
        {
            initialize();
            Severity = severity;
        }
        public questStatus(Severity severity, string message)
        {
            initialize();
            Severity = severity;
            Message = message;
        }
        public questStatus(Severity severity, string module, string method, string message)
        {
            Severity = severity;
            Module = module;
            Method = method;
            Message = message;
        }
        public questStatus(uint status, Severity severity)
        {
            initialize();
            Status = status;
            Severity = severity;
        }
        public questStatus(uint status, string message)
        {
            initialize();
            Status = status;
            Message = message;
        }
        public questStatus(uint status, Severity severity, string Message)
        {
            initialize();
            Status = status;
            Severity = severity;
        }
        public questStatus(string statusMessage)
        {
            if (statusMessage.IndexOf('|') != 1)
            {
                throw new ArgumentException(String.Format("Invalid statusMessage: {0}", statusMessage));
            }
            string[] _ss = statusMessage.Split('|');
            switch (_ss[0])
            {
                case "D":
                    this.Severity = Severity.Debug;
                    break;
                case "I":
                    this.Severity = Severity.Success;
                    break;
                case "W":
                    this.Severity = Severity.Warning;
                    break;
                case "E":
                    this.Severity = Severity.Error;
                    break;
                case "F":
                    this.Severity = Severity.Fatal;
                    break;
                default:
                    throw new ArgumentException(String.Format("Invalid statusMessage Severity: {0}   for message: {1}", _ss[0], statusMessage));
            }
            this.Message = _ss[1];
            initialize();
        }

        private void initialize()
        {
            StackFrame frame = new StackFrame(1);
            MethodBase method = frame.GetMethod();
            this.Module = method.DeclaringType.AssemblyQualifiedName;
            this.Method = method.Name;
            this.Message = "";
        }
    }
}
