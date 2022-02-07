using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Models.Mongo.GeoFence
{
    public class Polygon
    {
        [BsonElement("type")]
        public string Type { get; set; }
        [BsonElement("coordinates")]
        public List<List<List<double>>> Coordinates { get; set; }
    }
}