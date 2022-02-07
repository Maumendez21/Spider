using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Models.Mongo.GeoFence
{
    public class InterestPoint
    {
        [BsonElement("type")]
        public string Type { get; set; }
        [BsonElement("coordinate")]
        public List<double> Coordinate { get; set; }
        [BsonElement("radius")]
        public double Radius { get; set; }
    }
}