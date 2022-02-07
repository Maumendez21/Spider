using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.Main.Reports
{
    public class ReportsRequest
    {
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFin { get; set; }
    }
}