using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MasterPricing.DataMgr.Models
{
    public class FilterEditorViewModel : DataMgrBaseViewModel
    {
        public int Id { get; set; }
        public int TablesetId { get; set; }
        public int FilterId { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public List<BootstrapTreenodeViewModel> Entities { get; set; }
        public List<FilterItemViewModel> Items { get; set; }
        public ResultsOptionsViewModel _ResultsOptions { get; set; }
        public List<FilterProcedureViewModel> Procedures { get; set; }

        public FilterEditorViewModel()
            : base()
        {
            Entities = new List<BootstrapTreenodeViewModel>();
            Items = new List<FilterItemViewModel>();
            _ResultsOptions = new ResultsOptionsViewModel();
            Procedures = new List<FilterProcedureViewModel>();
        }
        public FilterEditorViewModel(UserSession userSession)
            : base(userSession)
        {
            Entities = new List<BootstrapTreenodeViewModel>();
            Items = new List<FilterItemViewModel>();
            _ResultsOptions = new ResultsOptionsViewModel();
            Procedures = new List<FilterProcedureViewModel>();
        }
        public FilterEditorViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Entities = new List<BootstrapTreenodeViewModel>();
            Items = new List<FilterItemViewModel>();
            _ResultsOptions = new ResultsOptionsViewModel();
            Procedures = new List<FilterProcedureViewModel>();
        }
    }
}