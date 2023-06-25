using gym.management.system.api.Interface;
using gym.management.system.api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace gym.management.system.api.Services
{
    public class MembersService : IMembersService
    {
        private readonly IConfiguration _configuration;
        private readonly IMongoCollection<Member> _membersCollection;


        public MembersService(IConfiguration configuration)
        {
            _configuration = configuration;
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("GMDConnection"));
            IMongoDatabase mongoDatabase = dbClient.GetDatabase(_configuration.GetConnectionString("DatabaseName"));
            _membersCollection = mongoDatabase.GetCollection<Member>("Member");
        }
        public async Task<List<Member>> AllMembersAsync()
        {
            try
            {
                var members = await _membersCollection.AsQueryable().ToListAsync();
                return members;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Member> AddMemberAsync(Member inputMember)
        {
            try
            {
                inputMember.Age = inputMember.Age ?? DateTime.Today.ToLocalTime().Year - inputMember.DateOfBirth.Year;
                if (DateTime.Today.ToLocalTime() < inputMember.DateOfBirth.AddYears(inputMember.Age ?? 0))
                {
                    inputMember.Age--;
                }
                if (inputMember.DateOfJoining is null)
                {
                    inputMember.DateOfJoining = DateTime.Now.ToLocalTime();
                }
                inputMember.QRCode = Convert.ToString(inputMember.DateOfJoining) + inputMember.Email + inputMember.Name;
                inputMember.Days = inputMember.Days ?? 0;

                //Insertion
                await _membersCollection.InsertOneAsync(inputMember);

                return inputMember;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Member> GetMemberByIdAsync(string id)
        {
            try 
            {
                var filter = Builders<Member>.Filter.Eq("_id", ObjectId.Parse(id));
                var member = await _membersCollection.FindAsync(filter).Result.FirstOrDefaultAsync();
                return member;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Member> CheckinMemberAsync(string qRCode)
        {
            try
            {
                FilterDefinition<Member> filter = Builders<Member>.Filter.Eq("QRCode", qRCode);
                var member = await _membersCollection.FindAsync(filter).Result.SingleOrDefaultAsync();
                if (member is null)
                {
                    throw new FileNotFoundException("Invalid QR Code");
                }

                if (member.CheckinStatusToday)
                {
                    throw new BadHttpRequestException($"{member.Name} is already Signed in for today");
                }
                else
                {
                    member.CheckinTimeToday = DateTime.Now.ToLocalTime();
                    var i = member.CheckinTimeToday.ToString();
                    UpdateDefinition<Member> update = Builders<Member>.Update.Set("CheckinStatusToday", true).Set("Days", ++member.Days).Set("CheckinTimeToday", member.CheckinTimeToday);
                    UpdateResult result = await _membersCollection.UpdateManyAsync(filter, update);
                    return member;
                }

            }
            catch (Exception)
            {
                throw;
            }

        }
        public async Task<Member> CheckoutMemberAsync(string qRCode)
        {
            try
            {
                FilterDefinition<Member> filter = Builders<Member>.Filter.Eq("QRCode", qRCode);
                var member = await _membersCollection.FindAsync(filter).Result.SingleOrDefaultAsync();
                if (member is null)
                {
                    throw new FileNotFoundException("Invalid QR Code");
                }

                if (!member.CheckinStatusToday)
                {
                    throw new BadHttpRequestException($"{member.Name} is not checkedin today");
                }
                else
                {
                    if (!member.CheckoutStatusToday)
                    {
                        member.CheckoutTimeToday = DateTime.Now.ToLocalTime();
                        UpdateDefinition<Member> update = Builders<Member>.Update.Set("CheckoutStatusToday", true).Set("CheckoutTimeToday", member.CheckoutTimeToday);
                        UpdateResult result = await _membersCollection.UpdateManyAsync(filter, update);
                        return member;
                    }
                    throw new BadHttpRequestException($"{member.Name} as already checked out");
                }

            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<bool> ResetMemberAttendance(Member member)
        {
            try
            {
                FilterDefinition<Member> filter = Builders<Member>.Filter.Eq("Id", member.Id);
                UpdateDefinition<Member> update = Builders<Member>.Update.Set("CheckinStatusToday", false).Set("CheckoutStatusToday", false);
                UpdateResult result = await _membersCollection.UpdateManyAsync(filter, update);

                return result.MatchedCount > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateMemberAsync(UpdateMember updateMember)
        {
            try
            {
                FilterDefinition<Member> filter = Builders<Member>.Filter.Eq("Id", updateMember.Id);
                var member = await _membersCollection.Find(filter).FirstOrDefaultAsync();
                if (updateMember.Days is not null) {
                    member.Days = updateMember.Days;
                }
                if (updateMember.Address is not null)
                {
                    member.Address = updateMember.Address;
                }
                if (updateMember.MobileNumber is not null)
                {
                    member.MobileNumber = updateMember.MobileNumber;
                }if (updateMember.HeightInCm is not null)
                {
                    member.HeightInCm = updateMember.HeightInCm??0;
                }if (updateMember.Weight is not null)
                {
                    member.Weight = updateMember.Weight??0;
                }
                var update = Builders<Member>.Update.Set("Days", member.Days)
                                       .Set("MobileNumber", member.MobileNumber)
                                       .Set("Address", member.Address)
                                       .Set("Weight", member.Weight)
                                       .Set("HeightInCm", member.HeightInCm);
                UpdateResult result = await _membersCollection.UpdateManyAsync(filter, update);

                return result.MatchedCount > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
