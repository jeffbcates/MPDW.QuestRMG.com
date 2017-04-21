using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Quest.MasterPricing.DataMgr.Models
{
    [Serializable]
    public class ColumnHeaderViewModel
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public string Type { get; set; }
        public int DisplayOrder { get; set; }
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

        public int? LookupId { get; set; }
        public int? TypeListId { get; set; }
    }
}