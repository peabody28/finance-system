using Microsoft.EntityFrameworkCore;
using payment.Interfaces.Repositories;

namespace payment.Repositories
{
    public class RepositoryBase : IRepositoryBase
    {
        private readonly DbContext dbContext;

        public RepositoryBase(DbContext dbContext)
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
