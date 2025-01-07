using System.IdentityModel.Tokens.Jwt;

namespace gym.management.system.api.Interface
{
    public interface IAuthService
    {
        Task<JwtSecurityToken> GenerateLoginToken(string username);
        Task<JwtSecurityToken> LoginAsync(string password, string username = "Admin");
        Task<bool> VerifyToken(string token);
    }
}
