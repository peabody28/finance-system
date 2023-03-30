using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;
using Testing.Integration.Core;
using currency.tests.Integration.Core.Constants;

namespace currency.tests.Integration.Core
{
    public class CurrencyWebApplicationFactory<TProgram, TContext> : AbstractWebApplicationFactory<TProgram, TContext>
        where TProgram : class
        where TContext : DbContext
    {
        public CurrencyWebApplicationFactory() 
        {
            UserName = TestUserConstants.UserName;
            UserRole = TestUserConstants.UserRole;
        }

        protected override void AddDbConnectionService(IServiceCollection services)
        {
            services.AddSingleton<DbConnection>(container =>
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                return connection;
            });
        }

        protected override void AddDbContextService(IServiceCollection services)
        {
            services.AddDbContext<TContext>((container, options) =>
            {
                var connection = container.GetRequiredService<DbConnection>();
                options.UseSqlite(connection);
            });
        }
    }
}
