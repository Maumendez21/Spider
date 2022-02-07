using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Mongo.RouteConsola
{
    public class Events
    {
        [BsonElement("id")]
        public int Id { get; set; }
        [BsonElement("start")]
        public DateTime Start { get; set; }
        [BsonElement("end")]
        public DateTime End { get; set; }
    }
}