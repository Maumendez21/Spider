using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace SpiderFleetWebAPI.Models.Mongo.Alarms
{
    public class Alarms
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
        [BsonElement("snAlarm")]
        public int SnAlarm { get; set; }
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
        [BsonElement("lastACC")]
        public DateTime LastACC { get; set; }
        [BsonElement("utcTime")]
        public DateTime UtcTime { get; set; }
        [BsonElement("totalMilage")]
        public int TotalMilage { get; set; }
        [BsonElement("currentMilage")]
        public int CurrentMilage { get; set; }
        [BsonElement("totalFuel")]
        public double TotalFuel { get; set; }
        [BsonElement("currentFuel")]
        public double CurrentFuel { get; set; }
        [BsonElement("vehiculeState")]
        public string VehiculeState { get; set; }
        [BsonElement("reservedByte")]
        public string ReservedByte { get; set; }
        [BsonElement("mode")]
        public string Mode { get; set; }
        [BsonElement("protocol")]
        public string Protocol { get; set; }
        [BsonElement("nAlarms")]
        public int NAlarms { get; set; }
        [BsonElement("alarms")]
        public List<Alarm> Alarmsn { get; set; }

    }
}
