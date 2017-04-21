using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MasterPricing.DataMgr.Models
{
    public class FolderListViewModel : BaseUserSessionViewModel
    {
        public int FolderId { get; set; }
        public List<BootstrapTreenodeViewModel> Items { get; set; }


        public FolderListViewModel()
        {
            Items = new List<BootstrapTreenodeViewModel>();
        }
    }
}