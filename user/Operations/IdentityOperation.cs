using System.Security.Claims;
using user.Helper;
using user.Interfaces.Operations;
using user.Interfaces.Repositories;

namespace user.Operations
{
    public class IdentityOperation : IIdentityOperation
    {
        private readonly IUserRepository UserRepository;

        public IdentityOperation(IUserRepository userRepository)
        {
            UserRepository = userRepository;
        }

        public ClaimsIdentity? Get(string name, string password)
        {
            var passwordHash = MD5Helper.Hash(password);
            var user = UserRepository.Get(name, passwordHash);

            if (user == null) return null;

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Code)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}
