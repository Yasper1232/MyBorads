namespace MyBorads.Entities
{
    public class State
    {
        public int Id { get; set; }

        public string Value { get; set; }
        public WorkItem WorkItem { get; set; }

        public WorkItem WorkItemId { get; set; }
    }
}
