namespace Application.DTOs
{
    public class AddBookDTO
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime PublishedDate { get; set; }
        public List<string> CategoryNames { get; set; }
    }
}