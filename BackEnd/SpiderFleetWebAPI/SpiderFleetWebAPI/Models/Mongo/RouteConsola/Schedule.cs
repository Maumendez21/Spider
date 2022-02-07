using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Mongo.RouteConsola
{
    public class Schedule
    {
        [BsonElement("id")]
        public int Id { get; set; }
        [BsonElement("concurrent")]
        public string concurrent { get; set; }
        [BsonElement("start")]
        public string Start { get; set; }
        [BsonElement("end")]
        public string End { get; set; }
    }
}