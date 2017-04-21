using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MasterPricing.Setup.Models
{
    public class LookupEditorViewModel : SetupBaseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string Summary { get; set; }
        public string SQL { get; set; }
        public string KeyField { get; set; }
        public string TextFields { get; set; }
        public string Arguments { get; set; }
        public int DatabaseId { get; set; }


        public LookupEditorViewModel() { }
        public LookupEditorViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public LookupEditorViewModel(UserSession userSession, BaseViewModel baseViewModel)
            : base(userSession, baseViewModel)
        {
        }
    }
}