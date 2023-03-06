using Microsoft.AspNetCore.Mvc;
using user.Interfaces.Operations;
using user.Models.Auth;

namespace user.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        #region [ Dependency -> Operations ]

        public IIdentityOperation IdentityOperation { get; set; }

        public IJwtTokenOperation JwtTokenOperation { get; set; }

        #endregion

        public AuthController(IIdentityOperation identityOperation, IJwtTokenOperation jwtTokenOperation)
        {
            IdentityOperation = identityOperation;
            JwtTokenOperation = jwtTokenOperation;
        }

        [HttpPost]
        public TokenModel Authozize(UserCredentialsModel model)
        {
            var identity = IdentityOperation.Get(model.Name, model.Password);
            // TODO: Validation
            if (identity == null)
                throw new BadHttpRequestException("Cannot find a user with specified credentials");

            var token = JwtTokenOperation.Generate(identity, out var expirationDate);

            return new TokenModel { AccessToken = token, ExpirationDate = expirationDate };
        }
    }
}
