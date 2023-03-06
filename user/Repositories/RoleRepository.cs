using user.Interfaces.Entities;
using user.Interfaces.Repositories;

namespace user.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly UserDbContext dbContext;

        public RoleRepository(UserDbContext userDbContext)
        {
            this.dbContext = userDbContext;
        }

        public IRole? Get(string code)
        {
            return dbContext.Role.FirstOrDefault(role => role.Code.Equals(code));
        }
    }
}
