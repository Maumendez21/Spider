using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.TravelReport
{
    public class TravelReport
    {
        public int Number { get; set; }
        public DateTime TravelDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Time { get; set; }
        public int HardAcceleration { get; set; }
        public int HardDeceleration { get; set; }
        public int HighRPM { get; set; }
        public int Speeding { get; set; }
        public double Distance { get; set; }
        public string Consumption { get; set; }
        public string Responsable { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
    }
}