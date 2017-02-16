using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.FMS
{
    public class IRS1099
    {
        public int Id { get; set; }
        public int TaxStatusId { get; set; }
        public string LegalName { get; set; }
        public string BusinessName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public DateTime Created { get; set; }
        public string EIN { get; set; }
    }
}
