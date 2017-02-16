using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.FMS
{
    public class TreenodeOption
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Label { get; set; }
        public string Image { get; set; }
        public bool bSelected { get; set; }

        public TreenodeOption()
        {
        }
        public TreenodeOption(string parentId, string id, string label)
        {
            Id = id;
            ParentId = parentId;
            Label = label;
        }
    }
}
