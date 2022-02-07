using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.RouteDiary
{
    public class RouteDiaryRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Notes { get; set; }
        public string Device { get; set; }
        public string IdRoute { get; set; }
        public string Frecuency { get; set; }
    }
}