using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.Util.Status;


namespace Quest.MPDW.Modelers
{
    public class UserMessageModeler
    {
        private questStatus status;
        private string _userMessage = null;

        public string UserMessage
        {
            get
            {
                if (_userMessage == null)
                {
                    buildUserMessage();
                }
                return (_userMessage);
            }
        }

        public UserMessageModeler(questStatus Status)
        {
            status = Status;
        }

        private void buildUserMessage()
        {
            _userMessage = String.Format("{0}|{1}", getSeverity(), status.Message);
        }
        private string getSeverity()
        {
            string _severity = Constants.keyUndefined;
            switch (status.Severity)
            {
                case Severity.Success:
                    _severity = Constants.keyInformational;
                    break;
                case Severity.Warning:
                    _severity = Constants.keyWarning;
                    break;
                case Severity.Error:
                    _severity = Constants.keyError;
                    break;
                case Severity.Fatal:
                    _severity = Constants.keyFatal;
                    break;
                default:
                    break;
            }
            return (_severity);
        }
    }
}