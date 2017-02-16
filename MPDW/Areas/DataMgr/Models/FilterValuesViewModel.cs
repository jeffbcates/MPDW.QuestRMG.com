using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MasterPricing.DataMgr.Models
{
    public class FilterValuesViewModel
    {
        public List<FilterItemViewModel> Items { get; set; }


        public FilterValuesViewModel()
        {
            Items = new List<FilterItemViewModel>();
        }
    }
}