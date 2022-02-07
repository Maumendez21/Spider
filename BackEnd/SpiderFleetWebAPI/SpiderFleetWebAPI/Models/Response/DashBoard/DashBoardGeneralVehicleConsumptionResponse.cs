using CredencialSpiderFleet.Models.DashBoard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.DashBoard
{
    public class DashBoardGeneralVehicleConsumptionResponse : BasicResponse
    {
        public Graficas GraficasConsumption { get; set; }

        public DashBoardGeneralVehicleConsumptionResponse()
        {
            GraficasConsumption = new Graficas();
        }
    }
}