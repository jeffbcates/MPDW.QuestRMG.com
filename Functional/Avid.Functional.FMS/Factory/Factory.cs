using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Status;


namespace Avid.Functional.FMS.Factory
{
    public class Factory
    {
        public questStatus GetOperator(string operatorType, out IFactoryInterface operatorObject)
        {
            // Initialize
            operatorObject = null;


            if (operatorType == null)
            {
                return (new questStatus(Severity.Error, "Missing operatorType"));
            }
            if (operatorType == "Alpha")
            {
                operatorObject = new OperatorAlpha();
            }
            else if (operatorType == "Beta")
            {
                operatorObject = new OperatorBeta();
            }
            else if (operatorType == "Gamma")
            {
                operatorObject = new OperatorGamma();
            }
            else
            {
                return (new questStatus(Severity.Error, "Invalid operatorType"));
            }
            return (new questStatus(Severity.Success));
        }
    }
}
