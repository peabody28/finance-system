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

        private readonly IIdentityOperation IdentityOperation;

        private readonly IJwtTokenOperation JwtTokenOperation;

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
