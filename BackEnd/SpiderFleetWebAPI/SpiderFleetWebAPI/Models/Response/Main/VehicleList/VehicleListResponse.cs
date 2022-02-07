using System.Collections.Generic;

namespace SpiderFleetWebAPI.Models.Response.Main.VehicleList
{
    public class VehicleListResponse:  BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Main.VehicleList.VehicleList> listVehiculos { get; set;}

        public VehicleListResponse()
        {
            listVehiculos = new List<CredencialSpiderFleet.Models.Main.VehicleList.VehicleList>();
        }
    }
}