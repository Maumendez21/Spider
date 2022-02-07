using CredencialSpiderFleet.Models.Models.Mongo.GeoFence;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SpiderFleetWebAPI.Models.Mongo.GeoFence
{
    public class GeoFence
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("hierarchy")]
        public string Hierarchy { get; set; }
        [BsonElement("active")]
        public bool Active { get; set; }
        [BsonElement("description")]
        public string Description { get; set; }
        [BsonElement("polygon")]
        public Polygon Polygon { get; set; }

    }
}