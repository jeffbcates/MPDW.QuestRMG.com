using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MasterPricing.DataMgr.Models
{
    public class FilterFolderViewModel : DataMgrBaseViewModel
    {
        public int Id { get; set; }
        public int FolderId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Created { get; set; }
        public string Modified { get; set; }
        public List<BootstrapTreenodeViewModel> Items { get; set; }

        public FilterFolderViewModel()
            : base()
        {
            Items = new List<BootstrapTreenodeViewModel>();
        }
        public FilterFolderViewModel(UserSession userSession)
            : base(userSession)
        {
            Items = new List<BootstrapTreenodeViewModel>();
        }
        public FilterFolderViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
            Items = new List<BootstrapTreenodeViewModel>();
        }
    }
}