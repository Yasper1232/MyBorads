namespace MyBorads.Entities
{
    public class Tag
    {
        public int Id { get; set; } //klucz glowny

        public string Value { get; set; }
      //  public string Category { get; set; }

        public virtual List<WorkItem> WorkItems { get; set; }


    }
}
