using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quest.MPDW.Models
{
    public class BootstrapTreenodeStates
    {
        public bool @checked { get; set; }
        public bool disabled { get; set; }
        public bool expanded { get; set; }
        public bool selected { get; set; }


        public BootstrapTreenodeStates()
        {
            this.@checked = false;
            this.disabled = false;
            this.expanded = false;
            this.selected = false;
        }
    }
}