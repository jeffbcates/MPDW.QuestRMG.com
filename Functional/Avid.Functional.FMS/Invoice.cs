using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.FMS
{
    public class Invoice
    {
        public int Id { get; set; }
        public int RequisitionId { get; set; }
        public int BidId { get; set; }
        public int ApplicationId { get; set; }
        public int JobId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Created { get; set; }
    }
}
