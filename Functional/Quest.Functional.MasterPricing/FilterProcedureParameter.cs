using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Quest.Functional.MasterPricing
{
    public class FilterProcedureParameter
    {
        public int Id { get; set; }
        public int FilterProcedureId { get; set; }
        public bool bRequired { get; set; }

        public string DbType { get; set; }
        public string Direction { get; set; }
        public bool? ForceColumnEncryption { get; set; }
        public bool IsNullable { get; set; }
        public int? LocaleId { get; set; }
        public int Offset { get; set; }
        public string ParameterName { get; set; }
        public byte[] Precision { get; set; }
        public byte[] Scale { get; set; }
        public int Size { get; set; }
        public string SourceColumn { get; set; }
        public bool? SourceColumnNullMapping { get; set; }
        public string SqlDbType { get; set; }
        public string TypeName { get; set; }
        public string UdtTypeName { get; set; }


        public FilterProcedureParameter()
        {
            Precision = new byte[1];
            Scale = new byte[1];
        }
    }
}
