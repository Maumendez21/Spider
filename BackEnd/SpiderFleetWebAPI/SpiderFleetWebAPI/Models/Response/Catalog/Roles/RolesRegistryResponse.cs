using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Catalog.Roles
{
    public class RolesRegistryResponse: BasicResponse
    {
        public CredencialSpiderFleet.Models.Catalogs.Roles.Roles roles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public RolesRegistryResponse()
        {
            roles = new CredencialSpiderFleet.Models.Catalogs.Roles.Roles();
        }
    }
}