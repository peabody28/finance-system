using wallet.Interfaces.Entities;

namespace wallet.Interfaces.Operations
{
    public interface IUserOperation
    {
        IUser? CurrentUser { get; }
    }
}
