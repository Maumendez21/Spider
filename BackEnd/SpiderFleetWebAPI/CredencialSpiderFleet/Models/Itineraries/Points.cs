using System;

namespace CredencialSpiderFleet.Models.Itineraries
{
    public class Points
    {
        public string events { get; set; }
        public DateTime Date { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string speed { get; set; }
    }
}