using Domain;
using System.Data;
using Dapper;

namespace ORM.Dapper
{
    public class PersonRepositoryDapper : IPersonRepository
    {
        private readonly IDbConnection dbConnection;

        public PersonRepositoryDapper(IDbConnection dbConnection)
        {
            dbConnection.Open();
            this.dbConnection = dbConnection;
        }

        // Ideally, since the use for this will never need the entirety of all Person, Address and Child grouped together (Children specifically doesn't need to be here)
        // there are better ways to do this
        public async Task<IEnumerable<Person>> GetPeople(int page, int recordsPerPage)
        {
            try
            {
                var sql =
                    @"SELECT p.Id as PersonId, p.FirstName, p.LastName, p.Age, a.PersonId, a.Address1, a.Address2, a.City, a.State, a.Zip, c.Id as ChildId, c.PersonId, c.FirstName, c.LastName, c.RelationshipType
                    FROM Person p
                    LEFT JOIN Address a ON a.PersonId = p.Id
                    LEFT JOIN Child c ON c.PersonId = p.Id";
                return await Query(sql);
            }
            catch (Exception ex)
            {
                // For debugging, put breakpoint here to see exception
                return null;
            }
        }

        // The community preferred way. Doing it in one query
        //public async Task<Person?> GetPerson(Guid id)
        //{
        //    try
        //    {
        //        var sql =
        //            @"SELECT p.Id as PersonId, p.FirstName, p.LastName, p.Age, a.PersonId, a.Address1, a.Address2, a.City, a.State, a.Zip, c.Id as ChildId, c.PersonId, c.FirstName, c.LastName, c.RelationshipType
        //            FROM Person p
        //            LEFT JOIN Address a ON a.PersonId = p.Id
        //            LEFT JOIN Child c ON c.PersonId = p.Id
        //            WHERE p.Id = @id";
        //        return (await Query(sql, new {id = id})).SingleOrDefault();
        //    }
        //    catch (Exception ex)
        //    {
        //        // For debugging, put breakpoint here to see exception
        //        return null;
        //    }
        //}

        // My preferred way. Using multiple queries, but only does one roundtrip over the network to the database
        public async Task<Person?> GetPerson(Guid id)
        {
            try
            {
                var sql =
                    @"SELECT p.Id as PersonId, p.FirstName, p.LastName, p.Age
                    FROM Person p
                    WHERE p.Id = @id;

                    SELECT a.Address1, a.Address2, a.City, a.State, a.Zip
                    FROM Address a
                    WHERE a.PersonId = @id;

                    SELECT c.Id as ChildId, c.FirstName, c.LastName, c.RelationshipType
                    FROM Child c
                    WHERE c.PersonId = @id;";
                var query = await dbConnection.QueryMultipleAsync(sql, new { id });
                var person = (await query.ReadAsync<Person>()).SingleOrDefault();
                if (person == null)
                {
                    return null;
                }

                person.Address = (await query.ReadAsync<Address>()).SingleOrDefault();
                person.Children = (await query.ReadAsync<Child>()).ToList();
                return person;
            }
            catch (Exception ex)
            {
                // For debugging, put breakpoint here to see exception
                return null;
            }
        }

        private async Task<IEnumerable<Person>> Query(string sql, object param = null)
        {
            var lookup = new Dictionary<Guid, Person>();
            var query = await dbConnection.QueryAsync<Person, Address, Child, Person>(
                sql,
                (p, a, c) =>
                {
                    p.Address = a;
                    if (!lookup.ContainsKey(p.PersonId))
                    {
                        lookup.Add(p.PersonId, p);
                    }
                    if (c != null)
                    {
                        var person = lookup[p.PersonId];
                        if (person.Children == null)
                        {
                            person.Children = new List<Child>();
                        }
                        person.Children.Add(c);
                    }
                    return p;
                },
                param = param,
                splitOn: "PersonId, PersonId, ChildId");

            return lookup.Values;
        }
    }
}
