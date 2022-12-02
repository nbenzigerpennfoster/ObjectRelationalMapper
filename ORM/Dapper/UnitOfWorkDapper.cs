using Dapper;
using Domain;
using System.Data;

namespace ORM.Dapper
{
    public class UnitOfWorkDapper : IUnitOfWork
    {
        private readonly IDbConnection dbConnection;

        public UnitOfWorkDapper(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public async Task Commit(Aggregate aggregate)
        {
            using (var transaction = dbConnection.BeginTransaction())
            {
                string sql = string.Empty;
                object sqlParams = null;
                // There's a way to append multiple event SQL statements together for one roundtrip to the db, but
                // I will leave that off for this example for simplicity
                foreach (var evnt in aggregate.Events())
                {
                    switch (evnt)
                    {
                        case PersonUpdatedEvent pue:
                            sql =
                                @$"DELETE FROM Address
                                WHERE PersonId = @personId;
    
                                {GetInsertAddressSql(pue.PersonUpdateData.Address)}

                                UPDATE Person
                                SET FirstName = @firstName,
                                LastName = @lastName,
                                Age = @age
                                WHERE Id = @personId;";
                            sqlParams = new
                            {
                                personId = pue.PersonUpdateData.PersonId,
                                address1 = pue.PersonUpdateData.Address?.Address1,
                                address2 = pue.PersonUpdateData.Address?.Address2,
                                city = pue.PersonUpdateData.Address?.City,
                                state = pue.PersonUpdateData.Address?.State,
                                zip = pue.PersonUpdateData.Address?.Zip,
                                firstName = pue.PersonUpdateData.FirstName,
                                lastName = pue.PersonUpdateData.LastName,
                                age = pue.PersonUpdateData.Age,
                            };
                            break;
                        case PersonAddedEvent pae:
                            sql =
                                @$"INSERT INTO Person (Id, FirstName, LastName, Age)
                                VALUES (@personId, @firstName, @lastName, @age);

                                {GetInsertAddressSql(pae.PersonAddData.Address)}";
                            sqlParams = new
                            {
                                personId = pae.PersonAddData.PersonId,
                                address1 = pae.PersonAddData.Address?.Address1,
                                address2 = pae.PersonAddData.Address?.Address2,
                                city = pae.PersonAddData.Address?.City,
                                state = pae.PersonAddData.Address?.State,
                                zip = pae.PersonAddData.Address?.Zip,
                                firstName = pae.PersonAddData.FirstName,
                                lastName = pae.PersonAddData.LastName,
                                age = pae.PersonAddData.Age,
                            };
                            break;
                        case PersonDeletedEvent pde:
                            sql =
                                @$"DELETE FROM Address
                                WHERE PersonId = @personId;
                                
                                DELETE FROM Person
                                WHERE Id = @personId;";
                            sqlParams = new
                            {
                                personId = pde.PersonDeleteData.PersonId,
                            };
                            break;
                    }
                }
                sql = sql + "SELECT @@ROWCOUNT;";
                try
                {
                    var rowCount = await dbConnection.ExecuteScalarAsync<int>(sql, sqlParams, transaction);
                    if (rowCount == 0)
                    {
                        // Nothing was updated because there were 0 recrods updated in the database table
                        transaction.Rollback();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    var x = 3;
                }
            }
        }

        private string GetInsertAddressSql(Address? address)
        {
            if (address == null)
            {
                return string.Empty;
            }

            return @"INSERT INTO Address (PersonId, Address1, Address2, City, State, Zip)
                   VALUES (@personId, @address1, @address2, @city, @state, @zip);";
        }
    }
}
