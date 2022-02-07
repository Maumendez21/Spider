using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Mongo.LogEngineStop
{
    public class LogEngineStop
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("node")]
        public string Node { get; set; }
        [BsonElement("device")]
        public string Device { get; set; }
        [BsonElement("user")]
        public string User { get; set; }
        [BsonElement("date")]
        public string Date { get; set; }
    }
}