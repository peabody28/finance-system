namespace payment.Interfaces.Repositories
{
    public interface IRepository
    {
        void BeginTransaction();

        void CommitTransaction();

        void RollbackTransaction();
    }
}
