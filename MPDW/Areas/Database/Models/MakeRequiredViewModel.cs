using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.Util.Data;


namespace Quest.MasterPricing.Database.Models
{
    public class MakeRequiredViewModel : DatabaseBaseListViewModel
    {
        public int StoredProcedureId { get; set; }
        public List<BaseId> Items { get; set; }


        public MakeRequiredViewModel()
        {
            Items = new List<BaseId>();
        }
    }
}