using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MasterPricing.DataMgr.Models
{
    public class FilterItemJoinViewModel
    {
        public int Id { get; set; }
        public int FilterItemId { get; set; }
        public string JoinType { get; set; }
        public string Identifier { get; set; }
        public string SourceSchema { get; set; }
        public string SourceEntityName { get; set; }
        public string SourceColumnName { get; set; }
        public int TargetEntityTypeId { get; set; }
        public string TargetSchema { get; set; }
        public string TargetEntityName { get; set; }
        public string TargetColumnName { get; set; }

        public int ColumnId { get; set; }
    }
}