using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book?> GetBookByIdAsync(Guid bookId);
        Task AddBookAsync(Book book);
        Task DeleteBookAsync(Guid bookId);
        Task UpdateBookAsync(Book book);
    }
}
