using Domain.Entities;
using Domain.Interfaces;
using Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace BookManagementUnitTests.RepositoryTests
{
    public class BookRepositoryTests : IClassFixture<DatabaseFixture>
    {
        private readonly ApplicationDbContext _context;
        private readonly IBookRepository _bookRepository;

        public BookRepositoryTests(DatabaseFixture fixture)
        {
            _context = fixture.Context;
            _bookRepository = new BookRepository(_context);
        }

        [Fact]
        public async Task GetAllBooksAsync_ReturnsAllBooks()
        {
            // Arrange
            SeedDatabase();

            // Act
            var books = await _bookRepository.GetAllBooksAsync();

            // Assert
            Assert.Equal(2, books.Count());
        }

        [Fact]
        public async Task GetBookByIdAsync_ReturnsBook()
        {
            // Arrange
            SeedDatabase();
            var bookId = _context.Books.First().BookId;

            // Act
            var book = await _bookRepository.GetBookByIdAsync(bookId);

            // Assert
            Assert.NotNull(book);
            Assert.Equal(bookId, book.BookId);
        }

        [Fact]
        public async Task AddBookAsync_AddsBook()
        {
            // Arrange
            SeedDatabase();
            var category = _context.Categories.First();
            var book = new Book
            {
                BookId = Guid.NewGuid(),
                Title = "New Book",
                Author = "New Author",
                PublishedDate = DateTime.Now,
                BookCategories = new List<BookCategory>
                {
                    new BookCategory { Category = category }
                }
            };

            // Act
            await _bookRepository.AddBookAsync(book);
            var addedBook = await _context.Books.FindAsync(book.BookId);

            // Assert
            Assert.NotNull(addedBook);
            Assert.Equal("New Book", addedBook.Title);
        }

        [Fact]
        public async Task DeleteBookAsync_DeletesBook()
        {
            // Arrange
            SeedDatabase();
            var bookId = _context.Books.First().BookId;

            // Act
            await _bookRepository.DeleteBookAsync(bookId);
            var deletedBook = await _context.Books.FindAsync(bookId);

            // Assert
            Assert.Null(deletedBook);
        }

        [Fact]
        public async Task UpdateBookAsync_UpdatesBook()
        {
            // Arrange
            SeedDatabase();
            var book = _context.Books.First();
            book.Title = "Updated Title";

            // Act
            await _bookRepository.UpdateBookAsync(book);
            var updatedBook = await _context.Books.FindAsync(book.BookId);

            // Assert
            Assert.NotNull(updatedBook);
            Assert.Equal("Updated Title", updatedBook.Title);
        }

        private void SeedDatabase()
        {
            _context.Books.RemoveRange(_context.Books);
            _context.Categories.RemoveRange(_context.Categories);
            _context.SaveChanges();

            var category1 = new Category { CategoryId = Guid.NewGuid(), Name = "Category 1" };
            var category2 = new Category { CategoryId = Guid.NewGuid(), Name = "Category 2" };

            var book1 = new Book
            {
                BookId = Guid.NewGuid(),
                Title = "Book 1",
                Author = "Author 1",
                PublishedDate = DateTime.Now,
                BookCategories = new List<BookCategory>
                {
                    new BookCategory { Category = category1 }
                }
            };

            var book2 = new Book
            {
                BookId = Guid.NewGuid(),
                Title = "Book 2",
                Author = "Author 2",
                PublishedDate = DateTime.Now,
                BookCategories = new List<BookCategory>
                {
                    new BookCategory { Category = category2 }
                }
            };

            _context.Books.AddRange(book1, book2);
            _context.Categories.AddRange(category1, category2);
            _context.SaveChanges();
        }
    }

    //To ensure that each test runs with a clean state, you can modify the BookRepositoryTests class to reset
    //the database before each test. This can be achieved by using the IClassFixture interface and the TestInitialize
    //method to clear the database before seeding it.

    //    Explanation:
    //1.	DatabaseFixture Class: This class sets up the in-memory database context and ensures it is disposed of after the tests are run.
    //2.	IClassFixture Interface: This interface is used to share the DatabaseFixture instance across all tests in the BookRepositoryTests class.
    //3.	SeedDatabase Method: This method is called at the beginning of each test to ensure the database is in a clean state before seeding it with the initial data.
    public class DatabaseFixture : IDisposable
    {
        public ApplicationDbContext Context { get; private set; }

        public DatabaseFixture()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "BookManagementTest")
                .Options;

            Context = new ApplicationDbContext(options);
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
