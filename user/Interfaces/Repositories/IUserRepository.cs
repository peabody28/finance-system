using user.Interfaces.Entities;

namespace user.Interfaces.Repositories
{
    public interface IUserRepository
    {
        IUser? Create(string name, string paswordHash, IRole role);

        IUser? Get(string name, string passwordHash);

        IUser? Get(string name);
    }
}
