using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.Functional.MasterPricing;


namespace Quest.MasterPricing.Database.Models
{
    public class ColumnLineItemViewModel
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
        public bool bIsAutoIncrement { get; set; }
        public bool bIsKey { get; set; }
        public bool bIsReadOnly { get; set; }
        public bool bIsRowVersion { get; set; }
        public bool bIsUnique { get; set; }
        public bool bIsHidden { get; set; }
        public bool bIsColumnSet { get; set; }
        public string Summary { get; set; }

        public int? LookupId { get; set; }
        public int? TypeListId { get; set; }
        public string Label { get; set; }
    }
}