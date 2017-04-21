using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quest.MPDW.Models.List
{
    public class QueryOptionsViewModel
    {
        public SortColumnListViewModel SortColumns { get; set; }
        public PagingOptionsViewModel Paging { get; set; }
        public SearchOptionsViewModel SearchOptions { get; set; }

        public QueryOptionsViewModel()
        {
            SortColumns = new SortColumnListViewModel();
            Paging = new PagingOptionsViewModel();
            SearchOptions = new SearchOptionsViewModel();
        }
        public QueryOptionsViewModel(int pageSize, int pageNumber)
            :this()
        {
            this.Paging.PageSize = pageSize;
            this.Paging.PageNumber = pageNumber;
        }
    }
}