using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Request.Roles
{
    public class RolesRequest
    {
        public int IdRole { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
    }
}