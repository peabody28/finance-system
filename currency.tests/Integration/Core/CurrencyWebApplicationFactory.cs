using currency.tests.Integration.Core.Constants;
using Testing.Integration.Helper.Sqlite;
using currency.Repositories;

namespace currency.tests.Integration.Core
{
    public class CurrencyWebApplicationFactory : SqliteContextWebApplicationFactory<Program, CurrencyDbContext>
    {
        public CurrencyWebApplicationFactory()
        {
            UserName = TestUserConstants.UserName;
            UserRole = TestUserConstants.UserRole;
        }
    }
}
