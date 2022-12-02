namespace Domain
{
    public abstract class Aggregate
    {
        protected Aggregate(string aggregateId)
        {
            AggregateId = aggregateId;
        }

        public string AggregateId { get; }
        protected List<string> errors = new List<string>();
        protected List<Event> events = new List<Event>();

        public bool HasErrors()
        {
            return errors.Count > 0;
        }

        public IEnumerable<string> Errors()
        {
            return errors;
        }

        public IEnumerable<Event> Events()
        {
            return events;
        }

        protected void RecordError(string error)
        {
            errors.Add(error);
        }

        protected void StageEvent(Event evnt)
        {
            events.Add(evnt);
        }
    }
}
