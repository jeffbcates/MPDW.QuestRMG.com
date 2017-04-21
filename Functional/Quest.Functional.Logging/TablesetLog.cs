using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.Logging
{
    public class TablesetLog
    {
        public int Id { get; set; }
        public int UserSessionId { get; set; }
        public string Username { get; set; }
        public string Database { get; set; }
        public string Name { get; set; }
        public string Event { get; set; }
        public string Data { get; set; }
        public DateTime Created { get; set; }
    }
}
