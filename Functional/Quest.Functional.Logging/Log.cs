using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.Logging
{
    public class Log
    {
        public int Id { get; set; }
        public int UserSessionId { get; set; }
        public int Severity { get; set; }
        public string Module { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; }
    }
}
