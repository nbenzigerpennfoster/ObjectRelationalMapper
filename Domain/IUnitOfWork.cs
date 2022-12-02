namespace Domain
{
    public interface IUnitOfWork
    {
        Task Commit(Aggregate aggregate);
    }
}
