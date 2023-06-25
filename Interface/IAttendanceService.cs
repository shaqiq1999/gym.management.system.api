using gym.management.system.api.Models;

namespace gym.management.system.api.Interface
{
    public interface IAttendanceService
    {
        public Task<bool> PostAttendanceByMemberDetails(Member member);
        public Task AttendanceSubmitAsync();
        public Task<List<Attendance>> GetAllAttendanceByIdAsync(string Id);
    }
}
