namespace MyBorads.Entities
{
    public class WorkItemTag
    {
        public int Id { get; set; }
        public WorkItem WorkItem { get; set; }
        public int WorkItemId { get; set; }

        public Tag Tag { get; set; }
        public int TagId { get; set; }

        public DateTime PublicationDate { get; set; }
    }
}
