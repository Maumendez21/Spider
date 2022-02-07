using CredencialSpiderFleet.Models.Models.Mongo.GeoFence;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace CredencialSpiderFleet.Models.Main.GeoFenceDevice
{
    public class GeoFenceDeviceList
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("hierarchy")]
        public string Hierarchy { get; set; }
        [BsonElement("polygon")]
        public Polygon Polygon { get; set; }
        public Dictionary<string, string> ListDevice { get; set; }

    }
}