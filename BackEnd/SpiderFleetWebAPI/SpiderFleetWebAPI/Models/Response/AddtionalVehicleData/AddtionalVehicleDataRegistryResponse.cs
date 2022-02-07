using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.AddtionalVehicleData
{
    public class AddtionalVehicleDataRegistryResponse: BasicResponse
    {
        public CredencialSpiderFleet.Models.AddtionalVehicleData.AddtionalVehicleDataRegistry addtional { get; set; }

        public AddtionalVehicleDataRegistryResponse ()
        {
            addtional = new CredencialSpiderFleet.Models.AddtionalVehicleData.AddtionalVehicleDataRegistry();
        }
    }
}