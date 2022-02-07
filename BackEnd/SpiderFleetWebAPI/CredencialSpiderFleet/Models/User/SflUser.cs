using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.User
{
    public class SflUser
    {
        public string ID_user { get; set; }
        public string ID_Node { get; set; }
        public string Password { get; set; }
        public string Passwordsalt { get; set; }
        public int Estado { get; set; }
        public string ID_U { get; set; }
        public int Edicion { get; set; }
        public string PasswordSpider { get; set; }
    }
}