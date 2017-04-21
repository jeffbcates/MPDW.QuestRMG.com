using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MasterPricing.Database.Models
{
    public class DatabaseBaseViewModel : BaseUserSessionViewModel
    {
        public int Id { get; set; }
        public string Server { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string ConnectionString { get; set; }
        public string LastRefresh { get; set; }


        public DatabaseBaseViewModel() { }
        public DatabaseBaseViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public DatabaseBaseViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
        }
    }
}