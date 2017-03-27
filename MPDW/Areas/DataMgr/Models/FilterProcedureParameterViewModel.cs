using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;


namespace Quest.MasterPricing.DataMgr.Models
{
    public class FilterProcedureParameterViewModel
    {
        public int Id { get; set; }
        public int FilterProcedureId { get; set; }
        public bool bRequired { get; set; }
        public string ParameterName { get; set; }
        public int Offset { get; set; }
        public string DbType { get; set; }
        public string Direction { get; set; }
        public string SqlDbType { get; set; }
        public int Size { get; set; }
        public bool IsNullable { get; set; }
        public bool ForceColumnEncryption { get; set; }
        public int LocaleId { get; set; }
        public byte Precision { get; set; }
        public byte Scale { get; set; }
        public string SourceColumn { get; set; }
        public bool SourceColumnNullMapping { get; set; }
        public string TypeName { get; set; }
        public string UdtTypeName { get; set; }


        public FilterProcedureParameterViewModel()
        {
        }
    }
}