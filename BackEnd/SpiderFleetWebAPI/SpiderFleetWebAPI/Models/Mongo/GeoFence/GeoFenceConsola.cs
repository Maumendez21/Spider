using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SpiderFleetWebAPI.Models.Mongo.RouteConsola;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Mongo.GeoFence
{
    public class GeoFenceConsola
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("device")]
        public string Device { get; set; }
        [BsonElement("fences")]
        public List<GeoFenceConsolas> Fences { get; set; }
    }
}