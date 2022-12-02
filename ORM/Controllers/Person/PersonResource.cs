namespace ORM.Controllers.Person
{
    public class PersonResource
    {
        public static PersonResource CreatePersonResource(Domain.Person person)
        {
            var address = person.Address == null ? null :
                new Address
                {
                    Address1 = person.Address.Address1,
                    Address2 = person.Address.Address2,
                    City = person.Address.City,
                    State = person.Address.State,
                    Zip = person.Address.Zip,
                };

            return new PersonResource
            {
                PersonId = person.PersonId,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Age = person.Age,
                Address = address
            };
        }

        public Guid PersonId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public Address? Address { get; set; }
    }
}
