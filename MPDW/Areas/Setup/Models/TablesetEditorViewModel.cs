using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;


namespace Quest.MasterPricing.Setup.Models
{
    public class TablesetEditorViewModel : SetupBaseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool bEnabled { get; set; }
        public string Summary { get; set; }
        public int DatabaseId { get; set; }


        public TablesetEditorViewModel() { }
        public TablesetEditorViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public TablesetEditorViewModel(UserSession userSession, BaseViewModel baseViewModel)
            : base(userSession, baseViewModel)
        {
        }
    }
}