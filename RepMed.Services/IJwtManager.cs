using RepMed.Core;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RepMed.Services
{

    public interface IJwtManager : IDisposable
    {
        JtwTokenResponse GenerateJWT(Guid userId, string email, string[] roles);
    }
    public class JwtManager : IJwtManager
    {
        private readonly AppSettings _appSettings;

        public JwtManager(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public JtwTokenResponse GenerateJWT(Guid userId, string email, string[] roles)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JwtAuth.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var Claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email,email),
                new Claim(ClaimTypes.NameIdentifier,userId.ToString())
            };
            var jwtClaims = Claims.ToDictionary(r => r.Type, r => (object)r.Value);
            Claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
            jwtClaims.Add(ClaimTypes.Role, roles);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _appSettings.JwtAuth.Issuer,
                Audience = _appSettings.JwtAuth.Issuer,
                TokenType = "Jwt",
                Claims = jwtClaims,
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = credentials
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new JtwTokenResponse
            {
                token = tokenHandler.WriteToken(token),
                validTill = tokenDescriptor.Expires.Value,
                Claims = Claims
            };
        }


    }
    public class JtwTokenResponse
    {
        public string token { get; set; }
        public DateTime validTill { get; set; }
        public IEnumerable<Claim> Claims { get; set; }
        
    }

}
