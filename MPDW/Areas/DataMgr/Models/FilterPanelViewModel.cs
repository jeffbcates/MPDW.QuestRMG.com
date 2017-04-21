using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.Functional.MasterPricing;


namespace Quest.MasterPricing.DataMgr.Models
{
    public class FilterPanelViewModel : DataMgrBaseViewModel
    {
        public string CurrentTab { get; set; }
        public FilterEditorViewModel Editor { get; set; }
        public List<BootstrapTreenodeViewModel> Entities { get; set; }
        public List<FilterItemViewModel> Items { get; set; }
        public List<FilterProcedureViewModel> Procedures { get; set; }


        public FilterPanelViewModel()
            : base()
        {
            Editor = new FilterEditorViewModel();
            Entities = new List<BootstrapTreenodeViewModel>();
            Items = new List<FilterItemViewModel>();
            Procedures = new List<FilterProcedureViewModel>();
        }
        public FilterPanelViewModel(UserSession userSession)
            : base(userSession)
        {
            Editor = new FilterEditorViewModel();
            Entities = new List<BootstrapTreenodeViewModel>();
            Items = new List<FilterItemViewModel>();
            Procedures = new List<FilterProcedureViewModel>();
        }
        public FilterPanelViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Editor = new FilterEditorViewModel();
            Entities = new List<BootstrapTreenodeViewModel>();
            Items = new List<FilterItemViewModel>();
            Procedures = new List<FilterProcedureViewModel>();
        }
    }
}