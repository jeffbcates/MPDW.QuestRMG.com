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
    
    public partial class Columns
    {
        public int Id { get; set; }
        public int EntityTypeId { get; set; }
        public int EntityId { get; set; }
        public int DisplayOrder { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }
        public string DataTypeName { get; set; }
        public int ColumnSize { get; set; }
        public bool bIsIdentity { get; set; }
        public bool bAllowDbNull { get; set; }
        public Nullable<bool> bIsAutoIncrement { get; set; }
        public Nullable<bool> bIsKey { get; set; }
        public Nullable<bool> bIsReadOnly { get; set; }
        public Nullable<bool> bIsRowVersion { get; set; }
        public Nullable<bool> bIsUnique { get; set; }
        public Nullable<bool> bIsHidden { get; set; }
        public string Summary { get; set; }
    }
}
