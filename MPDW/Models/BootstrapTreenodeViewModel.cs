using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quest.MPDW.Models
{
    public class BootstrapTreenodeViewModel
    {
        public int Id { get; set; }
        public int ParentId { get; set; }

        public string text { get; set; }
        public string icon { get; set; }
        public string selectedIcon { get; set; }
        public string color { get; set; }
        public string backColor { get; set; }
        public string href { get; set; }
        public string selectable { get; set; }
        public BootstrapTreenodeStates state { get; set; }
        public string[] tags { get; set; }
        public List<BootstrapTreenodeViewModel> nodes { get; set; }

        public string type { get; set; }
        public string parentType { get; set; }
        public string title { get; set; }
        public string Schema { get; set; }
        public string Name { get; set; }

        public BootstrapTreenodeViewModel()
        {
            tags = new string[0];
            nodes = new List<BootstrapTreenodeViewModel>();
            state = new BootstrapTreenodeStates();
        }
    }
}