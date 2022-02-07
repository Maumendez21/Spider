using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.Routes
{
    public class PointsCoordinates
    {
        public List<List<List<double>>> Coordinates { get; set; }
    }
}