using Testing.Integration.Helper.Sqlite;
using currency.Repositories;
using Microsoft.Extensions.DependencyInjection;
using currency.tests.Constants;

namespace currency.tests.Integration.Core
{
    public class CurrencyWebApplicationFactory : SqliteContextWebApplicationFactory<Program, CurrencyDbContext>
    {
        public CurrencyWebApplicationFactory()
        {
            UserName = TestUserConstants.UserName;
            UserRole = TestUserConstants.UserRole;
        }

        public CurrencyDbContext GetDbContext()
        {
            return Services.CreateScope().ServiceProvider.GetRequiredService<CurrencyDbContext>();
        }

        public void SetupDatabase()
        {
            var dbContext = Services.CreateScope().ServiceProvider.GetRequiredService<CurrencyDbContext>();
            dbContext.Database.EnsureCreated();
        }

        public void DeleteDatabase()
        {
            var dbContext = Services.CreateScope().ServiceProvider.GetRequiredService<CurrencyDbContext>();
            dbContext.Database.EnsureDeleted();
        }
    }
}
