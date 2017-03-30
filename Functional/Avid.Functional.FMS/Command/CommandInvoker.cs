using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Status;


namespace Avid.Functional.FMS.Command
{
    public class CommandInvoker 
    {
        private List<Command> _commandList = null;


        public CommandInvoker()
        {
            _commandList = new List<Command>();
        }

        public questStatus AddCommand(Command command)
        {
            _commandList.Add(command);
            return (new questStatus(Severity.Success));
        }
        public questStatus Create()
        {
            foreach (Command command in _commandList)
            {
                ICommand icommand = null;
                if (command.Type == "Alpha")
                {
                    icommand = new CommandAlpha();
                }
                else if (command.Type == "Beta")
                {
                    icommand = new CommandBeta();
                }
                else
                {
                    return (new questStatus(Severity.Error, String.Format("Invalid command: {0}", command.Type)));
                }
                icommand.Create(command.CommandObject);
            }
            return (new questStatus(Severity.Success));
        }
        public questStatus Read()
        {
            foreach (Command command in _commandList)
            {
                ICommand icommand = null;
                if (command.Type == "Alpha")
                {
                    icommand = new CommandAlpha();
                }
                else if (command.Type == "Beta")
                {
                    icommand = new CommandBeta();
                }
                else
                {
                    return (new questStatus(Severity.Error, String.Format("Invalid command: {0}", command.Type)));
                }
                icommand.Read(command.CommandObject);
            }
            return (new questStatus(Severity.Success));
        }
    }
}
