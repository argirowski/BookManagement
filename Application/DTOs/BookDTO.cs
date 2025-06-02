namespace Application.DTOs
{
    public class BookDTO
    {
        public Guid BookId { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public required DateTime PublishedDate { get; set; }
        public required List<string> CategoryNames { get; set; }
    }
}
