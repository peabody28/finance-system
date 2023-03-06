using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using user.Interfaces.Operations;

namespace user.Operations
{
    public class JwtTokenOperation : IJwtTokenOperation
    {
        public IConfiguration Configuration { get; set; }

        public JwtTokenOperation(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string Generate(ClaimsIdentity identity, out DateTime expirationDate)
        {
            var now = DateTime.UtcNow;
            var issuer = Configuration.GetSection("AuthOptions:ISSUER").Value;
            var audience = Configuration.GetSection("AuthOptions:AUDIENCE").Value;
            var lifetime = Configuration.GetSection("AuthOptions:LIFETIME").Get<double>();
            var key = Configuration.GetSection("AuthOptions:KEY").Value;
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
            expirationDate = now.Add(TimeSpan.FromMinutes(lifetime));

            var jwt = new JwtSecurityToken(issuer, audience, identity.Claims, now,
                expirationDate, new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
