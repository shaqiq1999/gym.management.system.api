using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace gym.management.system.api.Models
{
    public class UpdateMember
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string? MobileNumber { get; set; }
        public string? Address { get; set; }
        public int? HeightInCm { get; set; }
        public int? Weight { get; set; }
        public string? Email { get; set; }
        public int? Days { get; set; } 
        public List<string>? PaidMonths { get; set; } = new List<string>();
    }
}
