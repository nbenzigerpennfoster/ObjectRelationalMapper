using Dapper;
using Domain;
using EntityFramework;
using System.Data;

namespace ORM.Dapper
{
    public class PersonAggregateRepositoryDapper : IPersonAggregateRepository
    {
        private readonly IDbConnection dbConnection;

        public PersonAggregateRepositoryDapper(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public PersonAggregate GetPersonAggregate()
        {
            var person = new Person
            {
                PersonId = Guid.NewGuid()
            };

            return new PersonAggregate(person);
        }

        public async Task<PersonAggregate?> GetPersonAggregate(Guid personId)
        {
            try
            {
                var sql =
                    @"SELECT p.Id as PersonId
                    FROM Person p
                    WHERE p.Id = @id;

                    SELECT c.Id as ChildId, c.FirstName, c.LastName, c.RelationshipType
                    FROM Child c
                    WHERE c.PersonId = @id;";
                var query = await dbConnection.QueryMultipleAsync(sql, new { id = personId });
                var person = (await query.ReadAsync<Person>()).SingleOrDefault();
                if (person == null)
                {
                    return null;
                }

                person.Children = (await query.ReadAsync<Child>()).ToList();
                return new PersonAggregate(person);
            }
            catch (Exception ex)
            {
                // For debugging, put breakpoint here to see exception
                return null;
            }
        }
    }
}
