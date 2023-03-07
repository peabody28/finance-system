using wallet.Interfaces.Entities;
using wallet.Interfaces.Operations;
using wallet.Interfaces.Repositories;

namespace wallet.Operations
{
    public class UserOperation : IUserOperation
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        private readonly IUserRepository userRepository;

        public UserOperation(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userRepository = userRepository;
        }

        public IUser? CurrentUser
        {
            get
            {
                var name = httpContextAccessor.HttpContext.User?.Identity?.Name;
                if (string.IsNullOrWhiteSpace(name))
                    return null;

                return userRepository.Get(name);
            }
        }
    }
}
