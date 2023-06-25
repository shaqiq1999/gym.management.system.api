using gym.management.system.api.Models;

namespace gym.management.system.api.Interface
{
    public interface IMembersService
    {
        public Task<List<Member>> AllMembersAsync();
        public Task<Member> AddMemberAsync(Member inputMember);
        public Task<Member> CheckinMemberAsync(string qRCode);
        public Task<Member> CheckoutMemberAsync(string qRCode);
        public Task<bool> ResetMemberAttendance(Member member);
        public Task<Member> GetMemberByIdAsync(string id);
        public Task<bool> UpdateMemberAsync(UpdateMember updateMember);
    }
}
