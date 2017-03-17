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

        // Used by MPDW form submission
        public string RowLimit { get; set; }
        public string ColLimit { get; set; }

        // Optional. Fill these out if you want to export a filter NOT in the database.
        public List<FilterItemViewModel> Items { get; set; }
        public ResultsOptionsViewModel _ResultsOptions { get; set; }


        public FilterResultsExportViewModel()
            : base()
        {
            Items = new List<FilterItemViewModel>();
            _ResultsOptions = new ResultsOptionsViewModel();
        }
        public FilterResultsExportViewModel(UserSession userSession)
            : base(userSession)
        {
            Items = new List<FilterItemViewModel>();
            _ResultsOptions = new ResultsOptionsViewModel();
        }
        public FilterResultsExportViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Items = new List<FilterItemViewModel>();
            _ResultsOptions = new ResultsOptionsViewModel();
        }
    }
}