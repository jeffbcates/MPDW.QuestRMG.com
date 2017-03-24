using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Quest.MasterPricing.Database.Models
{
    public class StoredProcedureViewModel
    {
        public int Id { get; set; }
        public int DatabaseId { get; set; }
        public string Name { get; set; }
    }
}