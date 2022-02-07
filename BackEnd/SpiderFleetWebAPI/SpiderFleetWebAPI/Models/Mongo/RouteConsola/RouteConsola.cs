using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Mongo.RouteConsola
{
    public class RouteConsola
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("device")]
        public string Device { get; set; }
        [BsonElement("routes")]
        public List<RouteConsolas> Routes { get; set; }
    }
}