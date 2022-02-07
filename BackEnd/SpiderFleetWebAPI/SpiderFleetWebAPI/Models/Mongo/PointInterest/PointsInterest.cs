using CredencialSpiderFleet.Models.Models.Mongo.GeoFence;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Mongo.PointInterest
{
    public class PointsInterest
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("hierarchy")]
        public string Hierarchy { get; set; }
        [BsonElement("active")]
        public bool Active { get; set; }
        [BsonElement("description")]
        public string Description { get; set; }
        [BsonElement("interestPoints")]
        public InterestPoint InterestPoint { get; set; }
    }
}