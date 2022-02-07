using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Mongo
{
    public class InfoDevices
    {
        [BsonElement("_id")]
        public string _id { get; set; }
        [BsonElement("name")]
        public string name { get; set; }
        [BsonElement("date")]
        public DateTime date { get; set; }
        [BsonElement("location")]
        public Location location { get; set; }
        [BsonElement("event")]
        public string _event { get; set; }
        [BsonElement("group")]
        public List<string> group { get; set; }
    }
}