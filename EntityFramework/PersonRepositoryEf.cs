using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ORM.Controllers;

namespace EntityFramework
{
    public class PersonRepositoryEf : IPersonRepository
    {
        private readonly TinkeringDbContext tinkeringDbContext;

        public PersonRepositoryEf(string connectionString)
        {
            tinkeringDbContext = new TinkeringDbContext(connectionString);
        }

        public async Task<IEnumerable<Person>> GetPeople(int page, int recordsPerPage)
        {
            return await IncludeRelationships()
                .Skip((page - 1) * recordsPerPage)
                .Take(recordsPerPage)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Person> GetPerson(Guid id)
        {
            return await IncludeRelationships()
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.PersonId == id);
        }

        private IIncludableQueryable<Person, ICollection<Child>> IncludeRelationships()
        {
            return tinkeringDbContext.People
                .Include(p => p.Address)
                .Include(p => p.Children);
        }
    }
}
