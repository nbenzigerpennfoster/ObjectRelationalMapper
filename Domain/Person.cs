namespace Domain
{
    public class Person
    {
        public Guid PersonId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public Address? Address { get; set; }

        public ICollection<Child> Children { get; set; }
    }
}
