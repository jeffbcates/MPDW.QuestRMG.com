using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quest.MPDW.Models.List
{
    public class SearchOptionsViewModel
    {
        public List<SearchFieldViewModel> SearchFieldList { get; set; }
        public string SearchString { get; set; }
        public SearchOptionsViewModel()
        {
            SearchFieldList = new List<SearchFieldViewModel>();
            SearchString = "";
        }
    }
}