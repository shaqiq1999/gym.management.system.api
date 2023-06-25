using gym.management.system.api.Interface;
using gym.management.system.api.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace gym.management.system.api.Services
{
    public class AttendanceService : IAttendanceService
    {
        public readonly IMembersService _membersService;
        private readonly IConfiguration _configuration;
        private readonly IMongoCollection<Attendance> _attendanceCollection;
        public AttendanceService(IConfiguration configuration, IMembersService membersService)
        {
            _membersService = membersService;
            _configuration = configuration;
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("GMDConnection"));
            IMongoDatabase mongoDatabase = dbClient.GetDatabase(_configuration.GetConnectionString("DatabaseName"));
            _attendanceCollection = mongoDatabase.GetCollection<Attendance>("Attendance");
        }

        public async Task AttendanceSubmitAsync()
        {
            var Members = await _membersService.AllMembersAsync();
            var attendanceByMembers = new List<Attendance>();
            foreach (var member in Members)
            {
                if (member.CheckinStatusToday && member.CheckoutStatusToday)
                {
                    //Inserting all at once
                    var attendance = new Attendance()
                    {
                        CheckinTime = member.CheckinTimeToday,
                        CheckoutTime = member.CheckoutTimeToday,
                        MemberId = member.Id,
                        Name = member.Name
                    };
                    attendanceByMembers.Add(attendance);

                    //Inserting one at a time
                    //_ = await PostAttendanceByMemberDetails(member);
                    _ = await _membersService.ResetMemberAttendance(member);
                }
                else if (member.CheckinStatusToday)
                {
                    _ = await _membersService.ResetMemberAttendance(member);
                }
            }

            await PostAllAttendance(attendanceByMembers);
        }

        public async Task<bool> PostAttendanceByMemberDetails(Member member)
        {
            try
            {
                var attendance = new Attendance()
                {
                    CheckinTime = member.CheckinTimeToday,
                    CheckoutTime = member.CheckoutTimeToday,
                    MemberId = member.Id,
                    Name = member.Name,
                    //Id = Convert.ToString(member.CheckinTimeToday)
                };

                //Insertion
                await _attendanceCollection.InsertOneAsync(attendance);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task PostAllAttendance(IEnumerable<Attendance> attendances)
        {
            try
            {
                await _attendanceCollection.InsertManyAsync(attendances);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Attendance>> GetAllAttendanceByIdAsync(string Id)
        {
            try
            {
                var filter = Builders<Attendance>.Filter.Eq("MemberId", Id);
                var attendanceList = await _attendanceCollection.FindAsync(filter).Result.ToListAsync();
                return attendanceList;
            }
            catch (Exception) {
                throw;
            }
        }
    }
}
