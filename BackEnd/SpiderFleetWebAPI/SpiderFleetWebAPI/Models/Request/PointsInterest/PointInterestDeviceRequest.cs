using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.PointsInterest
{
    public class PointInterestDeviceRequest
    {
        public string IdPointInterest { get; set; }
        public List<string> ListDevice { get; set; }
    }
}