using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace CredencialSpiderFleet.Models.Main.TraceTrip
{
    public class Point
    {
        public string Device{ get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public double Speed { get; set; }
        public string Event { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}