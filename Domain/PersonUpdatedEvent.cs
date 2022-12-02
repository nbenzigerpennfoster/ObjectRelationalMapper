namespace Domain
{
    public class PersonUpdatedEvent : Event
    {
        public Person PersonUpdateData { get; set; }
    }
}
