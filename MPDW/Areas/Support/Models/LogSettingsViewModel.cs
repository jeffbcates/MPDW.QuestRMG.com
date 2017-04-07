using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MPDW.Support.Models
{
    public class LogSettingsViewModel : BaseUserSessionViewModel
    {
        public int Id { get; set; }
        public bool bLogHTTPRequests { get; set; }
        public bool bLogExceptions { get; set; }
        public bool bLogStoredProcedures { get; set; }
        public bool bLogDatabases { get; set; }
        public bool bLogTablesets { get; set; }
        public bool bLogFilters { get; set; }
        public bool bLogBulkInserts { get; set; }
        public bool bLogBulkUpdates { get; set; }
        public bool bLogPortal { get; set; }
        public bool bAllowLogUsers { get; set; }


        public LogSettingsViewModel()
            : base()
        { }
        public LogSettingsViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public LogSettingsViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
        }
    }
}