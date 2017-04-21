using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Status;


namespace Avid.Functional.FMS.Command
{
    public interface ICommand
    {
        questStatus Create(object commandObject);
        questStatus Read(object commandObject);
        questStatus Update(object commandObject);
        questStatus Delete(object commandObject);
        questStatus List(object commandObject);

    }
}
