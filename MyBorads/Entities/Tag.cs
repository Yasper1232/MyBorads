namespace MyBorads.Entities
{
    public class Tag
    {
        public int Id { get; set; } //klucz glowny

        public string Value { get; set; }
            
        public List<WorkItem> WorkItems { get; set; }


    }
}
