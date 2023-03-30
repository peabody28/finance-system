using Microsoft.EntityFrameworkCore;
using Testing.Integration.Helper.Sqlite;
using wallet.tests.Integration.Core.Constants;

namespace wallet.tests.Integration.Core
{
    public class WalletWebApplicationFactory<TProgram, TContext> : SqliteContextWebApplicationFactory<TProgram, TContext>
        where TProgram : class
        where TContext : DbContext
    {
        public WalletWebApplicationFactory()
        {
            UserName = TestUserConstants.UserName;
            UserRole = TestUserConstants.UserRole;
        }
    }
}
