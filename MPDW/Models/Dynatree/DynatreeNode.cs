using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quest.MPDW.Models
{
    public class DynatreeNode
    {
        public string title { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string Id { get; set; }
        public string datetime { get; set; }
        public string key { get; set; }
        public bool isFolder { get; set; }
        public bool isLazy { get; set; }
        public string tooltip { get; set; }
        public string icon { get; set; }
        public string addClass { get; set; }
        public bool noLink { get; set; }
        public bool activate { get; set; }
        public bool focus { get; set; }
        public bool expand { get; set; }
        public bool select { get; set; }
        public bool hideCheckbox { get; set; }
        public bool unselectable { get; set; }
        public List<DynatreeNode> children { get; set; }

        public DynatreeNode()
        {
            children = new List<DynatreeNode>();
        }
    }

}
