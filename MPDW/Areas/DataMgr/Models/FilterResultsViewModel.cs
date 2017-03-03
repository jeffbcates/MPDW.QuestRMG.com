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
    [Serializable]
    public class FilterResultsViewModel 
    {
        public List<ColumnHeaderViewModel> Columns = null;
        public List<DynamicRowViewModel> Items = null;



        public FilterResultsViewModel()
            : base()
        {
            Columns = new List<ColumnHeaderViewModel>();
            Items = new List<DynamicRowViewModel>();
        }
    }
}