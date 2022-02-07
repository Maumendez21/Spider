using CredencialSpiderFleet.Models.Models.Mongo.GeoFence;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Mongo.RouteConsola
{
    public class RouteConsolas
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("active")]
        public bool Active { get; set; }
        [BsonElement("schedule")]
        public List<Schedule> Schedule { get; set; }
        [BsonElement("events")]
        public List<Events> Events { get; set; }

    }
}