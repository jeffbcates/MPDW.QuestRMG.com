using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.Functional.MasterPricing;


namespace Quest.MasterPricing.Database.Models
{
    public class ParameterListViewModel : DatabaseBaseListViewModel
    {
        public int DatabaseId { get; set; }
        public DatabaseBaseViewModel Database { get; set; }
        public List<OptionValuePair> StoredProcedureOptions { get; set; }
        public string ParentEntityType { get; set; }
        public int StoredProcedureId { get; set; }
        public StoredProcedureViewModel StoredProcedure { get; set; }
        public List<ParameterLineItemViewModel> Items { get; set; }


        public ParameterListViewModel()
        {
            Database = new DatabaseBaseViewModel();
            StoredProcedureOptions = new List<OptionValuePair>();
            StoredProcedure = new StoredProcedureViewModel();
            Items = new List<ParameterLineItemViewModel>();
        }
        public ParameterListViewModel(UserSession userSession)
            : base(userSession)
        {
            Database = new DatabaseBaseViewModel();
            StoredProcedureOptions = new List<OptionValuePair>();
            StoredProcedure = new StoredProcedureViewModel();
            Items = new List<ParameterLineItemViewModel>();
            Items = new List<ParameterLineItemViewModel>();
        }
        public ParameterListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Database = new DatabaseBaseViewModel();
            StoredProcedureOptions = new List<OptionValuePair>();
            StoredProcedure = new StoredProcedureViewModel();
            Items = new List<ParameterLineItemViewModel>();
        }
    }
}