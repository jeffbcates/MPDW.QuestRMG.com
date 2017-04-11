using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.Logging
{
    public class LogSetting
    {
        public int Id { get; set; }
        public bool bLogHTTPRequests { get; set; }
        public bool bLogExceptions { get; set; }
        public bool bLogStoredProcedures { get; set; }
        public bool bLogDatabases { get; set; }
        public bool bLogTablesets { get; set; }
        public bool bLogFilters { get; set; }
        public bool bLogBulkInserts { get; set; }
        public bool bLogBulkInsertsPerRow { get; set; }
        public bool bLogBulkUpdates { get; set; }
        public bool bLogBulkUpdatesPerRow { get; set; }
        public bool bLogPortal { get; set; }
        public bool bAllowLogUsers { get; set; }
        public DateTime LastModified { get; set; }
    }
}
