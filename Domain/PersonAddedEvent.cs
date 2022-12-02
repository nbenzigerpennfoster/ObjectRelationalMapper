namespace Domain
{
    public class PersonAddedEvent : Event
    {
        public Person PersonAddData { get; set; }
    }
}
