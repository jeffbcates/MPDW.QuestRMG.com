using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.Logging
{
    public class BulkInsertLog
    {
        public int Id { get; set; }
        public int UserSessionId { get; set; }
        public string Username { get; set; }
        public string Database { get; set; }
        public string Tableset { get; set; }
        public string Filter { get; set; }
        public string Event { get; set; }
        public int NumRows { get; set; }
        public string Parameters { get; set; }
        public string BulkInsertColumn { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
        public DateTime Created { get; set; }
    }
}
