using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Quest.MPDW.Models;


namespace Quest.MPDW.Models
{
    public class LoginResponseViewModel : BaseViewModel
    {
        [Required]
        [EmailAddress]
        [StringLength(200, MinimumLength = 5)]
        public string Username { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string Password { get; set; }

        public bool bRememberMe { get; set; }
    }
}