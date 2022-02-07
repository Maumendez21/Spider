using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Mongo
{
    public class Trips
    {
        [BsonId]
        public ObjectId id { get; set; }
        [BsonElement("device")]
        public string device { get; set; }
        [BsonElement("name")]
        public string name { get; set; }
        [BsonElement("startDate")]
        public DateTime startDate { get; set; }
        [BsonElement("startAddress")]
        public string startAddress { get; set; }
        [BsonElement("startLocation")]
        public Location startLocation { get; set; }
        [BsonElement("endDate")]
        public DateTime endDate { get; set; }
        [BsonElement("endAddress")]
        public string endAddress { get; set; }
        [BsonElement("endLocation")]
        public Location endLocation { get; set; }
        [BsonElement("distance")]
        public int distance { get; set; }
        [BsonElement("fuel")]
        public double fuel { get; set; }
    }
}