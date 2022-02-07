using CredencialSpiderFleet.Models.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Permission
{
    public class ModulesResponse: BasicResponse
    {
        public List<Modules> modules { get; set; }

        public ModulesResponse ()
        {
            modules = new List<Modules>();
        }
    }
}