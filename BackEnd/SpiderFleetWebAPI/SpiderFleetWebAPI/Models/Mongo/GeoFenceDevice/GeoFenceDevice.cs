using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SpiderFleetWebAPI.Models.Mongo.GeoFenceDevice
{
    public class GeoFenceDevice
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("idGeoFence")]
        public string IdGeoFence { get; set; }
        [BsonElement("device")]
        public string Device { get; set; }
    }
}