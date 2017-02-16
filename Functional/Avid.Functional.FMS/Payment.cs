using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.FMS
{
    public class Payment
    {
        public int Id { get; set; }
        public int ProfileTypeId { get; set; }
        public int ProfileId { get; set; }
        public int RequisitionId { get; set; }
        public int? BidId { get; set; }
        public int? ApplicationId { get; set; }
        public int? JobId { get; set; }
        public int? InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Created { get; set; }
        public decimal InvoiceAmount { get; set; }
        public DateTime InvoiceCreated { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RequisitionName { get; set; }
        public string RequisitionDescription { get; set; }
        public DateTime RequisitionCreated { get; set; }
        public string RequisitionCreatedTimezone { get; set; }
        public DateTime? JobDeliveryDate { get; set; }
    }
}
