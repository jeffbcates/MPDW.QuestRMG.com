using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Quest.MPDW.Support.Models
{
    public class HTTPRequestLineItemViewModel
    {
        public int Id { get; set; }
        public int? UserSessionId { get; set; }
        public string Username { get; set; }
        public string Method { get; set; }
        public string IPAddress { get; set; }
        public string UserAgent { get; set; }
        public string URL { get; set; }
        public string Created { get; set; }
    }
}