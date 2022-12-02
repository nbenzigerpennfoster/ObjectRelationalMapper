namespace Domain
{
    public class Child
    {
        public Guid ChildId { get; set; }

        public Guid PersonId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int RelationshipType { get; set; }

        public Person Person { get; set; }
    }
}
