namespace MyBorads.Entities
{
    public class Comment
    {
        public int Id { get; set; } //klucz glowny

        public string Message { get; set; }
        public string Author { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
