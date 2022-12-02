namespace Domain
{
    public class PersonDeletedEvent : Event
    {
        public Person PersonDeleteData { get; set; }
    }
}
