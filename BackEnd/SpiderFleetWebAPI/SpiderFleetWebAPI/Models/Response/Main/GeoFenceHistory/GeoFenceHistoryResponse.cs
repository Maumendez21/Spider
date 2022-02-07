using CredencialSpiderFleet.Models.Main.GeoFenceHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Main.GeoFenceHistory
{
    public class GeoFenceHistoryResponse: BasicResponse
    {
        public List<PointsTimeOut> ListPointsTimeOut { get; set; }

        public GeoFenceHistoryResponse()
        {
            ListPointsTimeOut = new List<PointsTimeOut>();
        }
    }
}