using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.Catalog.TypeVehicle
{
    public class TypeVehicleUpdateRequest : TypeVehicleRequest
    {
        public int IdTypeVehicle { get; set; }
       
    }
}