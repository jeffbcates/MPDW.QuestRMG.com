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
    public class DeleteLogItemsViewModel : SupportBaseListViewModel
    {
        public List<BaseId> Items { get; set; }


        public DeleteLogItemsViewModel()
            : base()
        {
            Items = new List<BaseId>();
        }
        public DeleteLogItemsViewModel(UserSession userSession)
            : base(userSession)
        {
            Items = new List<BaseId>();
        }
        public DeleteLogItemsViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Items = new List<BaseId>();
        }
    }
}