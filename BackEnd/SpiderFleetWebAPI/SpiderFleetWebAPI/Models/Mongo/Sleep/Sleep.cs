using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SpiderFleetWebAPI.Models.Mongo.Sleep
{
    public class Sleep
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("serverTime")]
        public DateTime ServerTime { get; set; }
        [BsonElement("event")]
        public string Event { get; set; }
        [BsonElement("device")]
        public string Device { get; set; }
        [BsonElement("date")]
        public DateTime Date { get; set; }
        [BsonElement("location")]
        public Coordenate Location { get; set; }
        [BsonElement("speed")]
        public double Speed { get; set; }
        [BsonElement("bearing")]
        public double Bearing { get; set; }
        [BsonElement("version")]
        public int Version { get; set; }
        [BsonElement("utcTime")]
        public DateTime UtcTime { get; set; }
    }
}
