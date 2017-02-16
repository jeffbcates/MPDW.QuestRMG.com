using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Quest.MasterPricing.Database.Models
{
    public class ViewLineItemViewModel
    {
        public int Id { get; set; }
        public string Schema { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
    }
}