using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MPDW.Equipment.Models
{
    public class EquipmentBaseViewModel : BaseUserSessionViewModel
    {

        public EquipmentBaseViewModel() { }
        public EquipmentBaseViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public EquipmentBaseViewModel(UserSession userSession, BaseViewModel baseViewModel)
            : base(userSession, baseViewModel)
        {
        }
    }
}