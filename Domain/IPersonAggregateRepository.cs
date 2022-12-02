namespace Domain
{
    public interface IPersonAggregateRepository
    {
        PersonAggregate GetPersonAggregate();

        Task<PersonAggregate?> GetPersonAggregate(Guid personId);
    }
}
