using CredencialSpiderFleet.Models.Models.Mongo.GeoFence;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Mongo.Route
{
    public class TraceOfRoute
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("id")]
        public string Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("notes")]
        public string Notes { get; set; }
        [BsonElement("latitude")]
        public string Latitude { get; set; }
        [BsonElement("longitude")]
        public string Longitude { get; set; }
        [BsonElement("polygon")]
        public Polygon Polygon { get; set; }

    }
}