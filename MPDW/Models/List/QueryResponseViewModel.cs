using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quest.MPDW.Models.List
{
    public class QueryResponseViewModel
    {
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}