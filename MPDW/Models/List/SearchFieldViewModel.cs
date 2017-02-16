using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quest.MPDW.Models.List
{
    public class SearchFieldViewModel
    {
        public string Name { get; set; }
        public SearchOperationViewModel SearchOperation { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }

        public SearchFieldViewModel()
        {
            SearchOperation = new SearchOperationViewModel();
        }
    }
}