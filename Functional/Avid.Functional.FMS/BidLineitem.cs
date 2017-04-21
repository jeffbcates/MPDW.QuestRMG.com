using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avid.Util.Data;


namespace Avid.Functional.FMS
{
    public class BidLineItem
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RequisitionName { get; set; }
        public string RequisitionDescription { get; set; }
        public System.DateTime RequisitionCreated { get; set; }
        public string RequisitionCreatedTimezone { get; set; }
        public int Id { get; set; }
        public int BidderProfileTypeId { get; set; }
        public int BidderProfileId { get; set; }
        public int RequisitionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime Created { get; set; }
    }
}
