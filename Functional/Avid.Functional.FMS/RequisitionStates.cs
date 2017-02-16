using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.FMS
{
    public enum RequisitionStates
    {
        Draft = 1,
        Review = 2,
        Posted = 3,
        Bidding = 4,
        Awarded = 5,
        WorkInProgress = 6,
        Paid = 7,
        Complete = 8,
        Archived = 9
    }
}
