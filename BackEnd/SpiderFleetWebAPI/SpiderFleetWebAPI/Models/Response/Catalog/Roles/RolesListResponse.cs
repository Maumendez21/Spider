using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Catalog.Roles
{
    public class RolesListResponse : BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Catalogs.Roles.Roles> listRoles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public RolesListResponse()
        {
            listRoles = new List<CredencialSpiderFleet.Models.Catalogs.Roles.Roles>();
        }
    }
}