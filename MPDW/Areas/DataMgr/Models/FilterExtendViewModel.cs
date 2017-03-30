using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MasterPricing.DataMgr.Models
{
    public class FilterExtendViewModel : DataMgrBaseViewModel
    {
        public int FilterId { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public List<FilterItemViewModel> Items { get; set; }


        public FilterExtendViewModel()
            : base()
        {
            Items = new List<FilterItemViewModel>();
        }
        public FilterExtendViewModel(UserSession userSession)
            : base(userSession)
        {
            Items = new List<FilterItemViewModel>();
        }
        public FilterExtendViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Items = new List<FilterItemViewModel>();
        }
    }
}