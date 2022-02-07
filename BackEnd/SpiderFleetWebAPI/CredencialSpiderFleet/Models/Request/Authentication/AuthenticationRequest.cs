using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Request.Authentication
{
    public class AuthenticationRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public int type { get; set; }
    }
}