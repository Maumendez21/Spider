using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Main.TraceTrip
{
    public class GeneralInformation
    {
        //public DateTime Date { get; set; }
        //public int TotalMilage { get; set; }
        //public double TotalFuel { get; set; }
        //public decimal TotalTrips { get; set; }
        //public decimal TotalMinutes { get; set; }
        //public decimal TotalKm { get; set; }
        //public decimal Km { get; set; }
        //public string Device { get; set; }

        public string Device { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public double Speed { get; set; }
        public string Event { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public int milageStart { get; set; }
        public int milageEnd { get; set; }
        public double fuelStart { get; set; }
        public double fuelEnd { get; set; }
    }
}