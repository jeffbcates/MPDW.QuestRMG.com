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
    
    public partial class CustomerContacts
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public bool bActive { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
    }
}