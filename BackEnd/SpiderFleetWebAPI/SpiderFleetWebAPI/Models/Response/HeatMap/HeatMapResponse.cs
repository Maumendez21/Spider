using CredencialSpiderFleet.Models.HeatMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.HeatMap
{
    public class HeatMapResponse: BasicResponse
    {
        public List<Coordinates> Coords { get; set; }

        public HeatMapResponse()
        {
            Coords = new List<Coordinates>();
        }
    }
}