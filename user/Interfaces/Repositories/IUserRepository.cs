using user.Interfaces.Entities;

namespace user.Interfaces.Repositories
{
    public interface IUserRepository
    {
        IUser? Create(string name, string paswordHash, IRole role);
    }
}
