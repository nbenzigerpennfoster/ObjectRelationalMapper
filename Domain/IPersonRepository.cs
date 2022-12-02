namespace Domain
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetPeople(int page, int recordsPerPage);

        Task<Person?> GetPerson(Guid id);
    }
}
