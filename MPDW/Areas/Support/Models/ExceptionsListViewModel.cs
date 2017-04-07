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
    public class ExceptionsListViewModel : SupportBaseListViewModel
    {
        public List<ExceptionLineItemViewModel> Items { get; set; }


        public ExceptionsListViewModel()
            : base()
        {
            Items = new List<ExceptionLineItemViewModel>();
        }
        public ExceptionsListViewModel(UserSession userSession)
            : base(userSession)
        {
            Items = new List<ExceptionLineItemViewModel>();
        }
        public ExceptionsListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Items = new List<ExceptionLineItemViewModel>();
        }
    }
}