namespace Domain.Entities
{
    public class Book
    {
        public Guid BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime PublishedDate { get; set; }

        public ICollection<BookCategory> BookCategories { get; set; }
    }
}

