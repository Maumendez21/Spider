using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.AddtionalVehicleData
{
    public class AddtionalVehicleDataListResponse : BasicResponse
    {

        public List<CredencialSpiderFleet.Models.AddtionalVehicleData.AddtionalVehicleDataRegistry> ListAddtional { get; set; }

        public AddtionalVehicleDataListResponse()
        {
            ListAddtional = new List<CredencialSpiderFleet.Models.AddtionalVehicleData.AddtionalVehicleDataRegistry>();
        }
    }
}