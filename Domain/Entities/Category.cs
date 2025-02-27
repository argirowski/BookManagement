namespace Domain.Entities
{
    public class Category
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }

        public ICollection<BookCategory> BookCategories { get; set; }
    }
}
