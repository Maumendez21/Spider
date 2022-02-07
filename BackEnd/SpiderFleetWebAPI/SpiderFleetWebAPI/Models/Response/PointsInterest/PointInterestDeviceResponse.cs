using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.PointsInterest
{
    public class PointInterestDeviceResponse : BasicResponse
    {
        public Dictionary<string, string> resultados { get; set; }

        public PointInterestDeviceResponse()
        {
            resultados = new Dictionary<string, string>();
        }
    }
}