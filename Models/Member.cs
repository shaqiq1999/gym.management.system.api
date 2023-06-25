using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Reflection.Metadata.Ecma335;

namespace gym.management.system.api.Models
{
    public class Member
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public int HeightInCm { get; set; }
        public int Weight { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfJoining { get; set; } = DateTime.Today;
        public int? Days { get; set; } = 0;
        public bool CheckinStatusToday { get; set; } = false;
        public DateTime CheckinTimeToday { get; set; } = DateTime.Today;
        public bool CheckoutStatusToday { get; set; } = false;
        public DateTime CheckoutTimeToday { get; set; } = DateTime.Today;
        public List<string>? PaidMonths { get; set; } = new List<string>();
        public string? QRCode { get; set; }
    }
}
