namespace Domain.Entities
{
    public class Category
    {
        public Guid CategoryId { get; set; }
        public required string Name { get; set; }

        public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();
    }
}
