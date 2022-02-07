using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.ReportAdmin
{
    public class ReportItinerariosRequest
    {
        public DateTime inicio { get; set; }
        public DateTime fin { get; set; }
    }
}