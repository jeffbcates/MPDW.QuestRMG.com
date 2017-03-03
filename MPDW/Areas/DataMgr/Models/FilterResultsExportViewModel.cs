using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MasterPricing.DataMgr.Models
{
    public class FilterResultsExportViewModel : DataMgrBaseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RowLimit { get; set; }
        public string ColLimit { get; set; }


        public FilterResultsExportViewModel()
            : base()
        {
        }
        public FilterResultsExportViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public FilterResultsExportViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
        }
    }
}