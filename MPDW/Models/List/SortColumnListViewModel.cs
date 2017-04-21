using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quest.MPDW.Models.List
{
    public class SortColumnListViewModel
    {
        public List<SortColumnViewModel> Columns { get; set; }

        public SortColumnListViewModel()
        {
            Columns = new List<SortColumnViewModel>();
        }    
    }
}