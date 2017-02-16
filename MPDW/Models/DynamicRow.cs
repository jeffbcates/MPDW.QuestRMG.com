using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Quest.MasterPricing.DataMgr.Models
{
    public class DynamicRowViewModel
    {
        public List<ColumnValueViewModel> ColumnValues { get; set; }


        public DynamicRowViewModel()
        {
            ColumnValues = new List<ColumnValueViewModel>();
        }
    }
}