using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using OnlineEducationPlatform.Shared.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineEducationPlatform.Shared.Identity
{
    public class DefaultJwtSecurityTokenHandler(
        IConfiguration configuration,
        ILogger<DefaultJwtSecurityTokenHandler> logger) : JwtSecurityTokenHandler
    {
        protected readonly IConfiguration _configuration = configuration
            ?? throw new ArgumentNullException(nameof(configuration));
        protected readonly ILogger<DefaultJwtSecurityTokenHandler> _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));


        public string CreateToken(User user)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"] ?? throw new InvalidOperationException("SecretKey is missing in the settings."))),
                    SecurityAlgorithms.HmacSha256Signature),
                Audience = _configuration["JwtSettings:Audience"],
                Issuer = _configuration["JwtSettings:Issuer"]
            };

            var token = base.CreateJwtSecurityToken(tokenDescriptor);
            return WriteToken(token);
        }
    }
}