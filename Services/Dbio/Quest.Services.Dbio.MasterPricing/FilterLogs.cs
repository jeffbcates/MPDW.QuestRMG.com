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
    
    public partial class FilterLogs
    {
        public int Id { get; set; }
        public int UserSessionId { get; set; }
        public string Username { get; set; }
        public string Database { get; set; }
        public string Tableset { get; set; }
        public string Name { get; set; }
        public string Event { get; set; }
        public string Data { get; set; }
        public System.DateTime Created { get; set; }
    }
}
