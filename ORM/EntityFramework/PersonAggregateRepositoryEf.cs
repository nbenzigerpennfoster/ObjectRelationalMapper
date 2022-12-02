using Domain;
using EntityFramework;

namespace ORM.EntityFramework
{
    public class PersonAggregateRepositoryEf : IPersonAggregateRepository
    {
        private readonly IPersonRepository personRepository;
        private readonly TinkeringDbContext tinkeringDbContext;

        public PersonAggregateRepositoryEf(IPersonRepository personRepository, TinkeringDbContext tinkeringDbContext)
        {
            this.personRepository = personRepository;
            this.tinkeringDbContext = tinkeringDbContext;
        }

        public PersonAggregate GetPersonAggregate()
        {
            var person = new Person
            {
                PersonId = Guid.NewGuid()
            };
            tinkeringDbContext.Add(person);

            return new PersonAggregate(person);
        }

        public async Task<PersonAggregate?> GetPersonAggregate(Guid personId)
        {
            var person = await personRepository.GetPerson(personId);
            if (person == null)
            {
                return null;
            }
            tinkeringDbContext.Attach(person);

            return new PersonAggregate(person);
        }
    }
}
