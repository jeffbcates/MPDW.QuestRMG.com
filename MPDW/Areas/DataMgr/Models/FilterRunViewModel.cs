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
    public class FilterRunViewModel : DataMgrBaseListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FilterId { get; set; }
        public bool bExportToExcel { get; set; }
        public FilterResultsViewModel Results = null;

        // Optional. Fill these out if you want to trim down the full filter results.
        public List<FilterItemViewModel> Items { get; set; }
        public ResultsOptionsViewModel _ResultsOptions { get; set; }


        public FilterRunViewModel()
            : base()
        {
            Items = new List<FilterItemViewModel>();
            _ResultsOptions = new ResultsOptionsViewModel();
            Results = new FilterResultsViewModel();
        }
        public FilterRunViewModel(UserSession userSession)
            : base(userSession)
        {
            Items = new List<FilterItemViewModel>();
            _ResultsOptions = new ResultsOptionsViewModel();
            Results = new FilterResultsViewModel();
        }
        public FilterRunViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Items = new List<FilterItemViewModel>();
            _ResultsOptions = new ResultsOptionsViewModel();
            Results = new FilterResultsViewModel();
        }
    }
}