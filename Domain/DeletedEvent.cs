namespace Domain
{
    public abstract class DeletedEvent : Event
    {
        public abstract object GetDeletedEntity();
    }

    public abstract class DeletedEvent<T> : DeletedEvent where T : new()
    {
        public T DeletedEntity { get; set; }

        public override object GetDeletedEntity()
        {
            return DeletedEntity;
        }
    }
}
