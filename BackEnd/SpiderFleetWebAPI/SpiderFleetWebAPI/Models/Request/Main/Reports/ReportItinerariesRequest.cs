using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.Main.Reports
{
    public class ReportItinerariesRequest
    {
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }
        public string Dispositivo { get; set; }
    }
}