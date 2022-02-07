using System;

namespace CredencialSpiderFleet.Models.Itineraries
{
    public class ItinerariesKey
    {
        public int Diff { get; set; }
        public string Event { get; set; }
        public string Device { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ODO { get; set; }
        public string Fuel { get; set; }
        public string VelocidadMaxima { get; set; }
        public string NoAlarmas { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int totalM { get; set; }
        public double totalF { get; set; }
        public string label { get; set; }
        public string batery { get; set; }
    }
}