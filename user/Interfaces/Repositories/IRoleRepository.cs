using user.Interfaces.Entities;

namespace user.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        IRole? Get(string code);
    }
}
