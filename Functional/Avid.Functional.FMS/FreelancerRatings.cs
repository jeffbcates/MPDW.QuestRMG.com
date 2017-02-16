using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.FMS
{
    public class FrameworkRatings
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public int FrameworkId { get; set; }
        public int Rating { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string CreatedTimezone { get; set; }
    }
}
