using System.Security.Claims;

namespace user.Interfaces.Operations
{
    public interface IJwtTokenOperation
    {
        string Generate(ClaimsIdentity identity, out DateTime expirationDate);
    }
}
