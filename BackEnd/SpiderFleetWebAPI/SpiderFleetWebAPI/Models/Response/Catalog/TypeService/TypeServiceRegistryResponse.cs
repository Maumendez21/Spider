using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Catalog.TypeService
{
    public class TypeServiceRegistryResponse : BasicResponse
    {
        public CredencialSpiderFleet.Models.Catalogs.TypeService.TypeService typeService { get; set; }

        public TypeServiceRegistryResponse()
        {
            typeService = new CredencialSpiderFleet.Models.Catalogs.TypeService.TypeService();
        }
    }
}