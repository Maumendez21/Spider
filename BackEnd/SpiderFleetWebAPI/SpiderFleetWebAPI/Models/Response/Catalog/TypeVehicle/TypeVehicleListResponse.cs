using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Catalog.TypeVehicle
{
    public class TypeVehicleListResponse : BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Catalogs.TypeVehicle.TypeVehicle> ListTypeVehicles { get; set; }

        public TypeVehicleListResponse()
        {
            ListTypeVehicles = new List<CredencialSpiderFleet.Models.Catalogs.TypeVehicle.TypeVehicle>();
        }
    }
}