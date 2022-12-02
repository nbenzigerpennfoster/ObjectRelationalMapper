using Domain;
using EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace ORM.EntityFramework
{
    public class PersonRepositoryEf : IPersonRepository
    {
        private readonly TinkeringDbContext tinkeringDbContext;

        public PersonRepositoryEf(TinkeringDbContext tinkeringDbContext)
        {
            this.tinkeringDbContext = tinkeringDbContext;
        }

        public async Task<IEnumerable<Person>> GetPeople(int page, int recordsPerPage)
        {
            try
            {
                return await IncludeRelationships()
                    .Skip((page - 1) * recordsPerPage)
                    .Take(recordsPerPage)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<Person>();
            }
        }

        public async Task<Person?> GetPerson(Guid id)
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
