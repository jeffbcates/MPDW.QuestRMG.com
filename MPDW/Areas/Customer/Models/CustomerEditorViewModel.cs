using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;


namespace Quest.MPDW.Customer.Models
{
    public class CustomerEditorViewModel : CustomerBaseViewModel
    {
        public int Id { get; set; }
        public int CustomerTypeId { get; set; }
        public int RequisitionStateId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }


        public CustomerEditorViewModel() { }
        public CustomerEditorViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public CustomerEditorViewModel(UserSession userSession, BaseViewModel baseViewModel)
            : base(userSession, baseViewModel)
        {
        }
    }
}