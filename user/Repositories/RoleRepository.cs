using user.Interfaces.Entities;
using user.Interfaces.Repositories;

namespace user.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private UserDbContext DbContext { get; set; }

        public RoleRepository(UserDbContext userDbContext)
        {
            DbContext = userDbContext;
        }

        public IRole? Get(string code)
        {
            return DbContext.Role.FirstOrDefault(role => role.Code.Equals(code));
        }
    }
}
