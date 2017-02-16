using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MasterPricing.DataMgr.Models
{
    public class FilterItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public FilterEntityViewModel Entity { get; set; }
        public List<FilterOperationViewModel> Operations { get; set; }
        public List<FilterItemJoinViewModel> Joins { get; set; }
        public FilterItemLookupViewModel Lookup { get; set; }
        public FilterItemTypeListViewModel TypeList { get; set; }
        public string ParameterName { get; set; }

        public FilterEntityViewModel ParentEntity { get; set; }


        public FilterItemViewModel()
        {
            Entity = new FilterEntityViewModel();
            Operations = new List<FilterOperationViewModel>();
            Joins = new List<FilterItemJoinViewModel>();
            Lookup = new FilterItemLookupViewModel();
            TypeList = new FilterItemTypeListViewModel();
            ParentEntity = new FilterEntityViewModel();
        }
    }
}