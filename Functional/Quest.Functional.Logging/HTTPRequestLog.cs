using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.Logging
{
    public class HTTPRequestLog
    {
        public int Id { get; set; }
        public int UserSessionId { get; set; }
        public string Username { get; set; }
        public string IPAddress { get; set; }
        public string UserAgent { get; set; }
        public string URL { get; set; }
        public DateTime Created { get; set; }
    }
}
