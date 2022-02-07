using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Permission
{
    public class Permission
    {
        public string IdUser { get; set; }
        public string Modulo { get; set; }
        public bool Active { get; set; }
    }
}