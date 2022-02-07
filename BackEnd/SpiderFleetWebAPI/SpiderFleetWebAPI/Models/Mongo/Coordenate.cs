using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Mongo
{
    public class Coordenate
    {
        [BsonElement("type")]
        public string Type { get; set; }
        [BsonElement("coordinates")]
        public List<Double> Coordinates { get; set; }
    }
}