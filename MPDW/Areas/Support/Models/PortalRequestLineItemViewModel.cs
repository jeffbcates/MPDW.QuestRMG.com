using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Quest.MPDW.Support.Models
{
    public class PortalRequestLineItemViewModel
    {
        public int Id { get; set; }
        public int UserSessionId { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Event { get; set; }
        public string Data { get; set; }
        public string Created { get; set; }
    }
}