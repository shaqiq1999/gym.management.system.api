using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace gym.management.system.api.Models
{
    public class Attendance
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? MemberId { get; set; }
        public string Name { get; set; }
        public DateTime CheckinTime { get; set; }
        public DateTime CheckoutTime { get; set;}
    }
}
