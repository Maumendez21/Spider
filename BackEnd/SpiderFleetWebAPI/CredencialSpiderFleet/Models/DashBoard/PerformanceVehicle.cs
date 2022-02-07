using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.DashBoard
{
    public class PerformanceVehicle
    {
        public string Device { get; set; }
        public string Name { get; set; }
        public decimal Obd { get; set; }
        public decimal Fuel { get; set; }
    }
}