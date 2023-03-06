using System.Security.Claims;

namespace user.Interfaces.Operations
{
    public interface IIdentityOperation
    {
        ClaimsIdentity? Get(string name, string password);
    }
}
