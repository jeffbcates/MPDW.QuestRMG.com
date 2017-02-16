using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.FMS
{
    public class Employer
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public byte[] Logo { get; set; }
        public int? IRS1099Id { get; set; }
        public int CountryId { get; set; }
        public string Timezone { get; set; }
        public DateTime Created { get; set; }
    }
}
