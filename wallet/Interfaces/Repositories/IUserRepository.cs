using wallet.Interfaces.Entities;

namespace wallet.Interfaces.Repositories
{
    public interface IUserRepository
    {
        IUser Create(string name);

        IUser? Get(string name);

        IUser GetOrCreate(string name);
    }
}
