namespace Domain.Entities
{
    public class Book
    {
        public Guid BookId { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public required DateTime PublishedDate { get; set; }

        public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();
    }
}

