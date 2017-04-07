using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Quest.MPDW.Support.Models
{
    public class ExceptionLineItemViewModel
    {
        public int Id { get; set; }
        public int UserSessionId { get; set; }
        public string Username { get; set; }
        public string Module { get; set; }
        public string Message { get; set; }
        public string Created { get; set; }
    }
}