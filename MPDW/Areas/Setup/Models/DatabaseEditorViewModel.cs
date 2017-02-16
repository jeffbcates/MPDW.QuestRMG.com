using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.Util.Data;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MasterPricing.Setup.Models
{
    public class DatabaseEditorViewModel : SetupBaseViewModel
    {
        public int Id { get; set; }
        public string Server { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string ConnectionString { get; set; }


        public DatabaseEditorViewModel()
            : base()
        {
        }
        public DatabaseEditorViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public DatabaseEditorViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
        }
    }
}