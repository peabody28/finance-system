using Microsoft.EntityFrameworkCore;
using currency.tests.Integration.Core.Constants;
using Testing.Integration.Helper.Sqlite;

namespace currency.tests.Integration.Core
{
    public class CurrencyWebApplicationFactory<TProgram, TContext> : SqliteContextWebApplicationFactory<TProgram, TContext>
        where TProgram : class
        where TContext : DbContext
    {
        public CurrencyWebApplicationFactory()
        {
            UserName = TestUserConstants.UserName;
            UserRole = TestUserConstants.UserRole;
        }
    }
}
