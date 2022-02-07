using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Catalog.Mechanics
{
    public class MechanicsRegistryResponse : BasicResponse
    {
        public CredencialSpiderFleet.Models.Catalogs.Mechanics.Mechanics mechanics { get; set; }

        public MechanicsRegistryResponse()
        {
            mechanics = new CredencialSpiderFleet.Models.Catalogs.Mechanics.Mechanics();
        }
    }
}