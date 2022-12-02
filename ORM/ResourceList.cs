namespace ORM
{
    public class ResourceList<T> : Resource where T : class, new()
    {
        public IEnumerable<T> Resources { get; set; }
    }
}
