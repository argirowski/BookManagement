using Application.DTOs;
using Application.Features.Queries.GetBookById;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace BookManagementUnitTests.HandlerTests
{
    public class GetBookByIdQueryHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetBookByIdQueryHandler _handler;

        public GetBookByIdQueryHandlerTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetBookByIdQueryHandler(_bookRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsBook_WhenBookExists()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var book = new Book { BookId = bookId, Title = "Book 1", Author = "Author 1", PublishedDate = DateTime.Now };
            var bookDTO = new BookDTO { BookId = bookId, Title = "Book 1", Author = "Author 1", PublishedDate = DateTime.Now, CategoryNames = new List<string> { "Category 1" } };

            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(bookId)).ReturnsAsync(book);
            _mapperMock.Setup(m => m.Map<BookDTO>(book)).Returns(bookDTO);

            // Act
            var result = await _handler.Handle(new GetBookByIdQuery(bookId), CancellationToken.None);

            // Assert
            Assert.Equal(bookId, result.BookId);
            Assert.Equal("Book 1", result.Title);
        }

        [Fact]
        public async Task Handle_ReturnsNull_WhenBookDoesNotExist()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(bookId)).ReturnsAsync((Book)null);

            // Act
            var result = await _handler.Handle(new GetBookByIdQuery(bookId), CancellationToken.None);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Handle_ThrowsException_OnRepositoryError()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(bookId)).ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(new GetBookByIdQuery(bookId), CancellationToken.None));
        }
    }
}
