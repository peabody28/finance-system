using Microsoft.EntityFrameworkCore;
using payment.Interfaces.Repositories;

namespace payment.Repositories
{
    public class Repository : IRepository
    {
        private readonly DbContext dbContext;

        public Repository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void BeginTransaction()
        {
            dbContext.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            dbContext.Database.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            dbContext.Database.RollbackTransaction();
        }
    }
}
