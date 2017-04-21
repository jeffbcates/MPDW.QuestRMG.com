using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quest.Util.Data;


namespace Quest.Functional.ASM
{
    public class UserSession
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string IPAddress { get; set; }
        public string UserAgent { get; set; }
        public List<Privilege> Privileges { get; set; }
        public DateTime LastAction { get; set; }
        public DateTime Created { get; set; }
        public bool bLoggedOut { get; set; }
        public bool bTimedOut { get; set; }
        public DateTime? Terminated { get; set; }


        public UserSession()
        {
            User = new User();
            Privileges = new List<Privilege>();
        }
    }
}
