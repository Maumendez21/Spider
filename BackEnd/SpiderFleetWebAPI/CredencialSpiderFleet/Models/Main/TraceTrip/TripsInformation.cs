using System;

namespace CredencialSpiderFleet.Models.Main.TraceTrip
{
    public class TripsInformation
    {

        public string Device { get; set; }
        public int TotalMilage { get; set; }
        public double TotalFuel { get; set; }
        public int TotalTrips { get; set; }
        public DateTime Date { get; set; }
    }
}