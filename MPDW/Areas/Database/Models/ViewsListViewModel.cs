using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MasterPricing.Database.Models
{
    public class ViewsListViewModel : DatabaseBaseListViewModel
    {
        public int DatabaseId { get; set; }
        public List<ViewLineItemViewModel> Items { get; set; }


        public ViewsListViewModel()
        {
            Items = new List<ViewLineItemViewModel>();
        }
        public ViewsListViewModel(UserSession userSession)
            : base(userSession)
        {
            Items = new List<ViewLineItemViewModel>();
        }
        public ViewsListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Items = new List<ViewLineItemViewModel>();
        }
    }
}