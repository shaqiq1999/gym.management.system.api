using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace gym.management.system.api.Models
{
    public class Image
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string FileName { get; set; }
        public byte[] Data { get; set; }
    }
}
