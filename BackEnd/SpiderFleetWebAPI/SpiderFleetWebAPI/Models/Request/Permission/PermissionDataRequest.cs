using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.Permission
{
    public class PermissionDataRequest
    {
        public List<CredencialSpiderFleet.Models.Permission.Permission> PermissionList { get; set; }
    }
}