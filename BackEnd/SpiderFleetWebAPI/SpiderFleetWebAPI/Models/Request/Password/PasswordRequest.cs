using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.Password
{
    public class PasswordRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string OldPassword { get; set; }
    }
}