namespace Domain.Entities
{
    public class BookCategory
    {
        public Guid BookId { get; set; }
        public Book Book { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
