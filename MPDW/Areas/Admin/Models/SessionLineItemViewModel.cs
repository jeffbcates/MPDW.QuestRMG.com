using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Quest.MPDW.Admin.Models
{
    public class SessionLineItemViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Created { get; set; }
        public string Terminated { get; set; }
        public bool bLoggedOut { get; set; }
        public bool bTimedOut { get; set; }
    }
}