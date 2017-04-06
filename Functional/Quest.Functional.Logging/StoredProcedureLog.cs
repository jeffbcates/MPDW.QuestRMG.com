using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.Logging
{
    public class StoredProcedureLog
    {
        public int Id { get; set; }
        public int UserSessionId { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Arguments { get; set; }
        public string Response { get; set; }
        public DateTime Created { get; set; }
    }
}
