using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Functional.MasterPricing;


namespace Quest.MasterPricing.Services.Data
{
    public class FilterEntity
    {
        public FilterEntityType Type { get; set; }

        public FilterTable FilterTable { get; set; }
        public FilterView FilterView { get; set; }
        public string Alias { get; set; }

        public FilterEntity()
        {
            Type = new FilterEntityType();
            FilterTable = new FilterTable();
            FilterView = new FilterView();
        }           
    }
}
