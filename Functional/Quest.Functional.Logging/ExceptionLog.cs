using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.Logging
{
    public class ExceptionLog
    {
        public int Id { get; set; }
        public int UserSessionId { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }
        public string Module { get; set; }
        public string Method { get; set; }
        public string Status { get; set; }
        public string Data { get; set; }
        public string StackTrace { get; set; }
        public DateTime Created { get; set; }
    }
}
