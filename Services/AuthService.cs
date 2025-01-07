using gym.management.system.api.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace gym.management.system.api.Services
{
    public class AuthService : IAuthService
    {
        private readonly string _secretKey = "29AMJQo3dTGj9ZIr6SHX43BfbROyJlt77+e+y+4rwUI=";
        public async Task<JwtSecurityToken> GenerateLoginToken(string username)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username)
            };

            var token = new JwtSecurityToken(
                issuer: "GMSAdminIssuer",
                audience: "GMSAdminAudience",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);
            return token;
        }
        public async Task<JwtSecurityToken> LoginAsync(string password, string username = "Admin")
        {
            if (!string.IsNullOrEmpty(password) && password == "KeyPassword")
            {
                return await GenerateLoginToken(username);
            }
            else
            {
                throw new Exception("incorrect credentials");
            }
        }

        public async Task<bool> VerifyToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                }, out _);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
