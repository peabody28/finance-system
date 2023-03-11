using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using user.Constants;
using user.Interfaces.Operations;

namespace user.Operations
{
    public class JwtTokenOperation : IJwtTokenOperation
    {
        private readonly IConfiguration configuration;

        public JwtTokenOperation(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string Generate(ClaimsIdentity identity, out DateTime expirationDate)
        {
            var roleCode = identity.FindFirst(ClaimTypes.Role).Value;

            var now = DateTime.UtcNow;
            var issuer = configuration.GetSection("AuthOptions:ISSUER").Value;
            var audience = configuration.GetSection("AuthOptions:AUDIENCE").Value;
            var lifetime = configuration.GetSection("AuthOptions:LIFETIME").Get<double>();
            var key = configuration.GetSection("AuthOptions:KEY").Value;
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));

            expirationDate = roleCode.Equals(RoleConstants.MS) ? now.Add(TimeSpan.FromDays(365)) : now.Add(TimeSpan.FromMinutes(lifetime));

            var jwt = new JwtSecurityToken(issuer, audience, identity.Claims, now,
                expirationDate, new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
