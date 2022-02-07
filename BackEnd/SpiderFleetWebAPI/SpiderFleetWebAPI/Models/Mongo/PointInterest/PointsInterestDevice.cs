using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Mongo.PointInterest
{
    public class PointsInterestDevice
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("idPoint")]
        public string IdPoint { get; set; }
        [BsonElement("device")]
        public string Device { get; set; }
    }
}