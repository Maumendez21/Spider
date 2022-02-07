using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Catalog.TypeService
{
    public class TypeServiceListResponse : BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Catalogs.TypeService.TypeService> ListTypeServices { get; set; }

        public TypeServiceListResponse()
        {
            ListTypeServices = new List<CredencialSpiderFleet.Models.Catalogs.TypeService.TypeService>();
        }
    }
}