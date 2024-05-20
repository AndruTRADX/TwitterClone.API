using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TwitterClone.Models.Domains;

namespace TwitterClone.Repositories
{
    public class TokenRepository(IConfiguration configuration) : ITokenRepository
    {
        private readonly IConfiguration configuration = configuration;

        public string CreateJWTToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new("UserId", user.Id),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
