//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Quest.Services.Dbio.QuestERD
{
    using System;
    using System.Collections.Generic;
    
    public partial class ServicePrices
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string UOM { get; set; }
        public Nullable<decimal> Price { get; set; }
        public System.DateTime StartDateTime { get; set; }
        public Nullable<System.DateTime> EndDateTime { get; set; }
    }
}
