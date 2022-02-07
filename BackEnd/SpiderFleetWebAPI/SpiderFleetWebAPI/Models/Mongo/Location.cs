using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Mongo
{
    public class Location
    {
        [BsonElement("type")]
        public string type { get; set; }
        [BsonElement("coordinates")]
        public List<double> coordinates { get; set; }
    }
}