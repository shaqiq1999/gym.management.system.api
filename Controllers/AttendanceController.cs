using gym.management.system.api.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gym.management.system.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }
        [HttpGet]
        public async Task<ActionResult> DailyAttendanceSubmit()
        {
            await _attendanceService.AttendanceSubmitAsync();
            return Ok();
        }

        [HttpGet("getAttendanceById")]
        public async Task<ActionResult> GetAllAttendanceById([FromHeader] string Id)
        {
            return Ok(await _attendanceService.GetAllAttendanceByIdAsync(Id));
        }
    }
}
