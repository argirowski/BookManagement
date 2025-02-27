using Application.DTOs;
using Application.Features.Commands.UpdateBook;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace BookManagementUnitTests.HandlerTests
{
    public class UpdateBookCommandHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateBookCommandHandler _handler;

        public UpdateBookCommandHandlerTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new UpdateBookCommandHandler(_bookRepositoryMock.Object, _categoryRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_UpdatesBookSuccessfully()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var command = new UpdateBookCommand
            {
                BookId = bookId,
                Title = "Updated Book",
                Author = "Updated Author",
                PublishedDate = DateTime.Now,
                CategoryNames = new List<string> { "Category 1" }
            };

            var categories = new List<Category>
            {
                new Category { CategoryId = Guid.NewGuid(), Name = "Category 1" }
            };

            var book = new Book
            {
                BookId = bookId,
                Title = "Old Book",
                Author = "Old Author",
                PublishedDate = DateTime.Now.AddYears(-1),
                BookCategories = new List<BookCategory>()
            };

            var updatedBook = new Book
            {
                BookId = bookId,
                Title = command.Title,
                Author = command.Author,
                PublishedDate = command.PublishedDate,
                BookCategories = categories.Select(c => new BookCategory { Category = c }).ToList()
            };

            var bookDTO = new BookDTO
            {
                BookId = bookId,
                Title = command.Title,
                Author = command.Author,
                PublishedDate = command.PublishedDate,
                CategoryNames = command.CategoryNames
            };

            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(bookId)).ReturnsAsync(book);
            _categoryRepositoryMock.Setup(r => r.GetAllCategoriesAsync()).ReturnsAsync(categories);
            _bookRepositoryMock.Setup(r => r.UpdateBookAsync(It.IsAny<Book>())).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<BookDTO>(It.IsAny<Book>())).Returns(bookDTO);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _bookRepositoryMock.Verify(r => r.UpdateBookAsync(It.IsAny<Book>()), Times.Once);
            Assert.Equal(bookDTO.BookId, result.BookId);
            Assert.Equal(bookDTO.Title, result.Title);
        }

        [Fact]
        public async Task Handle_ThrowsKeyNotFoundException_WhenBookDoesNotExist()
        {
            // Arrange
            var command = new UpdateBookCommand
            {
                BookId = Guid.NewGuid(),
                Title = "Updated Book",
                Author = "Updated Author",
                PublishedDate = DateTime.Now,
                CategoryNames = new List<string> { "Category 1" }
            };

            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(command.BookId)).ReturnsAsync((Book)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ThrowsException_OnRepositoryError()
        {
            // Arrange
            var command = new UpdateBookCommand
            {
                BookId = Guid.NewGuid(),
                Title = "Updated Book",
                Author = "Updated Author",
                PublishedDate = DateTime.Now,
                CategoryNames = new List<string> { "Category 1" }
            };

            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(command.BookId)).ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
