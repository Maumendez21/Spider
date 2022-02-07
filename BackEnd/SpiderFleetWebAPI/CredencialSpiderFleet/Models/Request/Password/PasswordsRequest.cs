using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Request.Password
{
    public class PasswordsRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public int type { get; set; }
    }
}