using gym.management.system.api.Interface;
using gym.management.system.api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gym.management.system.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly IMembersService _memberService;
        private readonly IImageService _imageService;

        public MembersController(IMembersService membersService, IImageService imageService)
        {
            _memberService = membersService;
            _imageService = imageService;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllMembers()
        {
            return Ok(await _memberService.AllMembersAsync());
        }

        [HttpPost]
        public async Task<ActionResult> PostMember([FromBody] Member member)
        {
            return Ok(await _memberService.AddMemberAsync(member));
        }

        [HttpGet("member/id")]
        public async Task<ActionResult> GetMemberById([FromHeader] string id)
        {
            return Ok(await _memberService.GetMemberByIdAsync(id));
        }

        [HttpPost("image")]
        public async Task<ActionResult> PostImage([FromForm] IFormFile file,[FromForm] string memberId)
        {
            return Ok(await _imageService.UploadImage(file, memberId));
        }
        [HttpGet("image")]
        public async Task<ActionResult> GetImage([FromHeader] string memberId)
        {
            return Ok(await _imageService.GetImageAsync(memberId));
        }

        [HttpPut("checkin")]
        public async Task<ActionResult> CheckinMember([FromHeader]string qRCode)
        {
            return Ok(await _memberService.CheckinMemberAsync(qRCode));
        }
        [HttpPut("checkout")]
        public async Task<ActionResult> CheckoutMember([FromHeader] string qRCode)
        {
            return Ok(await _memberService.CheckoutMemberAsync(qRCode));
        }
        [HttpPut("update")]
        public async Task<ActionResult> UpdateMember([FromBody] UpdateMember updateMember)
        {
            return Ok(await _memberService.UpdateMemberAsync(updateMember));
        }
        [HttpPut("image")]
        public async Task<ActionResult> UpdateImage([FromBody] Image image)
        {
            return Ok(await _imageService.UpdateMemberImageAsync(image));
        }
    }
}
