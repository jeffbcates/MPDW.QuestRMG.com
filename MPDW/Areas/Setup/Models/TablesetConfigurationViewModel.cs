using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.MasterPricing;


namespace Quest.MasterPricing.Setup.Models
{
    public class TablesetConfigurationViewModel : SetupBaseViewModel
    {
        public int Id { get; set; }
        public int TablesetId { get; set; }
        public Tableset Tableset { get; set; }
        public Quest.Functional.MasterPricing.Database Database { get; set; }
        public List<BootstrapTreenodeViewModel> TableList { get; set; }
        public List<BootstrapTreenodeViewModel> ViewList { get; set; }
        public List<BootstrapTreenodeViewModel> DBTableList { get; set; }
        public List<BootstrapTreenodeViewModel> DBViewList { get; set; }



        public TablesetConfigurationViewModel()
        {
            Tableset = new Tableset();
            Database = new Quest.Functional.MasterPricing.Database();
            TableList = new List<BootstrapTreenodeViewModel>();
            ViewList = new List<BootstrapTreenodeViewModel>();
            DBTableList = new List<BootstrapTreenodeViewModel>();
            DBViewList = new List<BootstrapTreenodeViewModel>();
        }
        public TablesetConfigurationViewModel(UserSession userSession)
            : base(userSession)
        {
            Tableset = new Tableset();
            Database = new Quest.Functional.MasterPricing.Database();
            TableList = new List<BootstrapTreenodeViewModel>();
            ViewList = new List<BootstrapTreenodeViewModel>();
            DBTableList = new List<BootstrapTreenodeViewModel>();
            DBViewList = new List<BootstrapTreenodeViewModel>();
        }
        public TablesetConfigurationViewModel(UserSession userSession, BaseViewModel baseViewModel)
            : base(userSession, baseViewModel)
        {
            Tableset = new Tableset();
            Database = new Quest.Functional.MasterPricing.Database();
            TableList = new List<BootstrapTreenodeViewModel>();
            ViewList = new List<BootstrapTreenodeViewModel>();
            DBTableList = new List<BootstrapTreenodeViewModel>();
            DBViewList = new List<BootstrapTreenodeViewModel>();
        }
    }
}