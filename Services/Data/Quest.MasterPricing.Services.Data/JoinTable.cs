using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Functional.MasterPricing;


namespace Quest.MasterPricing.Services.Data
{
    public class JoinEntity
    {
        public FilterEntityType Type { get; set; }
        public FilterItem FilterItem { get; set; }

        public FilterTable FilterTable { get; set; }
        public FilterView FilterView { get; set; }
        public string Alias { get; set; }

        public FilterColumn FilterColumn { get; set; }
        public TablesetColumn TablesetColumn { get; set; }
        public FilterItemJoin FilterItemJoin { get; set; }


        public JoinEntity()
        {
            Type = new FilterEntityType();
            FilterItem = new FilterItem();
            FilterTable = new FilterTable();
            FilterView = new FilterView();
            TablesetColumn = new TablesetColumn();
            FilterColumn = new FilterColumn();
            FilterItemJoin = new FilterItemJoin();
        }
    }
}
