using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.Util.Data;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MPDW.Support.Models
{
    public class FiltersListViewModel : SupportBaseListViewModel
    {
        public List<FilterLineItemViewModel> Items { get; set; }


        public FiltersListViewModel()
            : base()
        {
            Items = new List<FilterLineItemViewModel>();
        }
        public FiltersListViewModel(UserSession userSession)
            : base(userSession)
        {
            Items = new List<FilterLineItemViewModel>();
        }
        public FiltersListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Items = new List<FilterLineItemViewModel>();
        }
    }
}