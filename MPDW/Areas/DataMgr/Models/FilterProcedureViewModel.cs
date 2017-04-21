using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MasterPricing.DataMgr.Models
{
    public class FilterProcedureViewModel
    {
        public int Id { get; set; }
        public int FilterId { get; set; }
        public string Action { get; set; }
        public string Name { get; set; }
        public List<FilterProcedureParameterViewModel> Parameters { get; set; }


        public FilterProcedureViewModel()
        {
            Parameters = new List<FilterProcedureParameterViewModel>();
        }
    }
}