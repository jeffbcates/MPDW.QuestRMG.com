using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.Util.Data;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MasterPricing.Setup.Models
{
    public class TypeListsListViewModel : SetupBaseListViewModel
    {
        public List<TypeListLineItemViewModel> Items { get; set; }


        public TypeListsListViewModel()
            : base()
        {
            Items = new List<TypeListLineItemViewModel>();
        }
        public TypeListsListViewModel(UserSession userSession)
            : base(userSession)
        {
            Items = new List<TypeListLineItemViewModel>();
        }
        public TypeListsListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Items = new List<TypeListLineItemViewModel>();
        }
    }
}