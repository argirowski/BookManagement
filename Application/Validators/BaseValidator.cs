using Domain.Interfaces;
using FluentValidation;

namespace Application.Validators
{
    public abstract class BaseValidator<T> : AbstractValidator<T>
    {
        protected readonly IBookRepository _bookRepository;
        protected readonly ICategoryRepository _categoryRepository;

        protected BaseValidator(IBookRepository bookRepository, ICategoryRepository categoryRepository)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
        }

        protected bool BookExists(Guid bookId)
        {
            var book = _bookRepository.GetBookByIdAsync(bookId).Result;
            return book != null;
        }

        protected bool AddedBookTitleExists(string title)
        {
            var existingBooks = _bookRepository.GetAllBooksAsync().Result;
            return existingBooks.Any(book => book.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }

        protected bool UpdatedBookTitleExists(Guid bookId, string title)
        {
            var existingBooks = _bookRepository.GetAllBooksAsync().Result;
            return existingBooks.Any(book => book.Title.Equals(title, StringComparison.OrdinalIgnoreCase) && book.BookId != bookId);
        }

        protected bool CategoriesExist(List<string> categoryNames)
        {
            var existingCategories = _categoryRepository.GetAllCategoriesAsync().Result;
            var existingCategoryNames = existingCategories.Select(c => c.Name).ToList();

            return categoryNames.All(name => existingCategoryNames.Contains(name));
        }
    }
}