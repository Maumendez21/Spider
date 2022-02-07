using MongoDB.Bson.Serialization.Attributes;

namespace SpiderFleetWebAPI.Models.Mongo
{
    public class Alarm
    {
        [BsonElement("n")]
        public int N { get; set; }
        [BsonElement("type")]
        public string Type { get; set; }
        [BsonElement("mark")]
        public string Mark { get; set; }
        [BsonElement("current")]
        public double Current { get; set; }
        [BsonElement("limit")]
        public double Limit { get; set; }
    }
}