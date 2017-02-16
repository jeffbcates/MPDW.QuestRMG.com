using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.Util.Data;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MPDW.Equipment.Models
{
    public class EquipmentsListViewModel : EquipmentBaseViewModel
    {
        public List<EquipmentsListViewModel> Items { get; set; }
        public QueryOptions QueryOptions { get; set; }


        public EquipmentsListViewModel()
            : base()
        {
            Items = new List<EquipmentsListViewModel>();
            QueryOptions = new QueryOptions();
        }
        public EquipmentsListViewModel(UserSession userSession)
            : base(userSession)
        {
            Items = new List<EquipmentsListViewModel>();
            QueryOptions = new QueryOptions();
        }
        public EquipmentsListViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Items = new List<EquipmentsListViewModel>();
            QueryOptions = new QueryOptions();
        }
    }
}