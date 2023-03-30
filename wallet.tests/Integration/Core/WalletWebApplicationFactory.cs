using Testing.Integration.Helper.Sqlite;
using wallet.Repositories;
using wallet.tests.Integration.Core.Constants;

namespace wallet.tests.Integration.Core
{
    public class WalletWebApplicationFactory : SqliteContextWebApplicationFactory<Program, WalletDbContext>
    {
        public WalletWebApplicationFactory()
        {
            UserName = TestUserConstants.UserName;
            UserRole = TestUserConstants.UserRole;
        }
    }
}
