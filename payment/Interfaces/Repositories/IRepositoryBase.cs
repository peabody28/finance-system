namespace payment.Interfaces.Repositories
{
    public interface IRepositoryBase
    {
        void BeginTransaction();

        void CommitTransaction();

        void RollbackTransaction();
    }
}
