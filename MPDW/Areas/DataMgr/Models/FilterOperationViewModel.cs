using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MasterPricing.DataMgr.Models
{
    public class FilterOperationViewModel
    {
        public int Id { get; set; }
        public int Operator { get; set; }
        public List<FilterValueViewModel> Values { get; set; }


        public FilterOperationViewModel()
        {
            Values = new List<FilterValueViewModel>();
        }
    }
}