using Application.DTOs;
using Application.Features.Queries.GetAllBooks;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace BookManagementUnitTests.HandlerTests
{
    public class GetAllBooksQueryHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllBooksQueryHandler _handler;

        public GetAllBooksQueryHandlerTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetAllBooksQueryHandler(_bookRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsListOfBooks()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { BookId = Guid.NewGuid(), Title = "Book 1", Author = "Author 1", PublishedDate = DateTime.Now },
                new Book { BookId = Guid.NewGuid(), Title = "Book 2", Author = "Author 2", PublishedDate = DateTime.Now }
            };
            var bookDTOs = new List<BookDTO>
            {
                new BookDTO { BookId = books[0].BookId, Title = "Book 1", Author = "Author 1", PublishedDate = DateTime.Now, CategoryNames = new List<string> { "Category 1" } },
                new BookDTO { BookId = books[1].BookId, Title = "Book 2", Author = "Author 2", PublishedDate = DateTime.Now, CategoryNames = new List<string> { "Category 2" } }
            };

            _bookRepositoryMock.Setup(r => r.GetAllBooksAsync()).ReturnsAsync(books);
            _mapperMock.Setup(m => m.Map<IEnumerable<BookDTO>>(books)).Returns(bookDTOs);

            // Act
            var result = await _handler.Handle(new GetAllBooksQuery(), CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("Book 1", result.First().Title);
            Assert.Equal("Book 2", result.Last().Title);
        }

        [Fact]
        public async Task Handle_ReturnsEmptyList_WhenNoBooks()
        {
            // Arrange
            var books = new List<Book>();
            var bookDTOs = new List<BookDTO>();

            _bookRepositoryMock.Setup(r => r.GetAllBooksAsync()).ReturnsAsync(books);
            _mapperMock.Setup(m => m.Map<IEnumerable<BookDTO>>(books)).Returns(bookDTOs);

            // Act
            var result = await _handler.Handle(new GetAllBooksQuery(), CancellationToken.None);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task Handle_ThrowsException_OnRepositoryError()
        {
            // Arrange
            _bookRepositoryMock.Setup(r => r.GetAllBooksAsync()).ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(new GetAllBooksQuery(), CancellationToken.None));
        }
    }
}
