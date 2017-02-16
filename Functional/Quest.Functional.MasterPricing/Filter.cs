using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Functional.MasterPricing
{
    public class Filter
    {
        public int Id { get; set; }
        public int TablesetId { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string SQL { get; set; }

        public List<FilterTable> FilterTableList { get; set; }
        public List<FilterView> FilterViewList { get; set; }
        public List<FilterColumn> FilterColumnList { get; set; }

        public List<FilterItem> FilterItemList { get; set; }
        public List<FilterProcedure> FilterProcedureList { get; set; }


        public Filter()
        {
            FilterTableList = new List<FilterTable>();
            FilterViewList = new List<FilterView>();
            FilterColumnList = new List<FilterColumn>();
            FilterItemList = new List<FilterItem>();
            FilterProcedureList = new List<FilterProcedure>();
        }
    }
}
