using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Catalog.TypeVehicle
{
    public class TypeVehicleRegistryResponse : BasicResponse
    {
        public CredencialSpiderFleet.Models.Catalogs.TypeVehicle.TypeVehicle TypeVehicle { get; set; }

        public TypeVehicleRegistryResponse()
        {
            TypeVehicle = new CredencialSpiderFleet.Models.Catalogs.TypeVehicle.TypeVehicle();
        }
    }
}