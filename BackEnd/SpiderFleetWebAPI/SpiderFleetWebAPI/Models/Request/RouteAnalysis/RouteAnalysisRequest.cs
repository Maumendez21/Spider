using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.RouteAnalysis
{
    public class RouteAnalysisRequest
    {
        public string Id { get; set; }
        public string Device { get; set; }
        public List<List<double>> Coordinates { get; set; }
    }
}