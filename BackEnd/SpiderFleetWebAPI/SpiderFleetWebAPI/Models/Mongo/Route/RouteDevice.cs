using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Mongo.Route
{
    public class RouteDevice
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("idRoute")]
        public string IdRoute { get; set; }
        [BsonElement("idRegistry")]
        public int IdRegistry { get; set; }
        [BsonElement("device")]
        public string Device { get; set; }
        [BsonElement("start")]
        public DateTime Start { get; set; }
        [BsonElement("end")]
        public DateTime End { get; set; }
    }
}