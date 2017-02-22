using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.Functional.MasterPricing;


namespace Quest.MasterPricing.DataMgr.Models
{
    public class DataMgrTablesetViewModel : DataMgrBaseViewModel
    {
        public int Id { get; set; }
        public int TablesetId { get; set; }
        public TablesetViewModel Tableset { get; set; }
        public List<BootstrapTreenodeViewModel> TableList { get; set; }
        public List<BootstrapTreenodeViewModel> ViewList { get; set; }
        public List<BootstrapTreenodeViewModel> Lookups { get; set; }
        public List<BootstrapTreenodeViewModel> TypeLists { get; set; }


        public DataMgrTablesetViewModel()
            : base()
        {
            Tableset = new TablesetViewModel();
            TableList = new List<BootstrapTreenodeViewModel>();
            ViewList = new List<BootstrapTreenodeViewModel>();
            Lookups = new List<BootstrapTreenodeViewModel>();
            TypeLists = new List<BootstrapTreenodeViewModel>();
        }
        public DataMgrTablesetViewModel(UserSession userSession)
            : base(userSession)
        {
            Tableset = new TablesetViewModel();
            TableList = new List<BootstrapTreenodeViewModel>();
            ViewList = new List<BootstrapTreenodeViewModel>();
            Lookups = new List<BootstrapTreenodeViewModel>();
            TypeLists = new List<BootstrapTreenodeViewModel>();
        }
        public DataMgrTablesetViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Tableset = new TablesetViewModel();
            TableList = new List<BootstrapTreenodeViewModel>();
            ViewList = new List<BootstrapTreenodeViewModel>();
            Lookups = new List<BootstrapTreenodeViewModel>();
            TypeLists = new List<BootstrapTreenodeViewModel>();
        }
    }
}