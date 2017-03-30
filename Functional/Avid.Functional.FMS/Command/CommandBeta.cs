using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Status;


namespace Avid.Functional.FMS.Command
{
    public class CommandBeta : ICommand
    {
        questStatus ICommand.Create(object commandObject)
        {
            return (new questStatus(Severity.Warning, "Not implemented"));
        }
        questStatus ICommand.Read(object commandObject)
        {
            return (new questStatus(Severity.Warning, "Not implemented"));
        }

        questStatus ICommand.Update(object commandObject)
        {
            return (new questStatus(Severity.Warning, "Not implemented"));
        }
        questStatus ICommand.Delete(object commandObject)
        {
            return (new questStatus(Severity.Warning, "Not implemented"));
        }
        questStatus ICommand.List(object commandObject)
        {
            return (new questStatus(Severity.Warning, "Not implemented"));
        }


        public CommandBeta()
        {
        }
    }
}
