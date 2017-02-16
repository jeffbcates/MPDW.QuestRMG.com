using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.FMS
{
    public class RequisitionAttachment
    {
        public int Id { get; set; }
        public int RequisitionId { get; set; }
        public string Tag { get; set; }
        public string MIMEType { get; set; }
        public int Filesize { get; set; }
        public string Filename { get; set; }
        public byte[] FileContents { get; set; }
        public DateTime Created { get; set; }
        public int? MessageId { get; set; }
    }
}
