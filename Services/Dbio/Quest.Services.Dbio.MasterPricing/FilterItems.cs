//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Quest.Services.Dbio.MasterPricing
{
    using System;
    using System.Collections.Generic;
    
    public partial class FilterItems
    {
        public int Id { get; set; }
        public int FilterId { get; set; }
        public int FilterEntityTypeId { get; set; }
        public int FilterEntityId { get; set; }
        public Nullable<int> LookupId { get; set; }
        public Nullable<int> TypeListId { get; set; }
        public string Label { get; set; }
        public string ParameterName { get; set; }
        public bool bHidden { get; set; }
    }
}
