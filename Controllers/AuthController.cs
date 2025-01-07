using gym.management.system.api.Interface;
using gym.management.system.api.Models;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace gym.management.system.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;  
        }

        [HttpPost("generate/username")]
        public async Task<IActionResult> GenerateToken(string username)
        {
            JwtSecurityToken token = await _authService.GenerateLoginToken(username);

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            JwtSecurityToken token = await _authService.LoginAsync(login.Password);
            var responseToken = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new TokenModel()
            {
                Token = responseToken
            });
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyToken([FromBody] TokenModel token)
        {   
            var isValid = await _authService.VerifyToken(token.Token);
            if (isValid)
            {
                return Ok(isValid);
            }
            else
            {
                return Unauthorized(isValid);
            }
        }
     
    }
}
