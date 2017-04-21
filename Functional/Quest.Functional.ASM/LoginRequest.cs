using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.ASM  
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool bRememberMe { get; set; }

        public string IPAddress { get; set; }
        public string UserAgent { get; set; }
    }
}
