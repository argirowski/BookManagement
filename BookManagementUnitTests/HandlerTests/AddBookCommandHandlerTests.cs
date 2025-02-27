using Application.DTOs;
using Application.Features.Commands.AddBook;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace BookManagementUnitTests.HandlerTests
{
    public class AddBookCommandHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AddBookCommandHandler _handler;

        public AddBookCommandHandlerTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new AddBookCommandHandler(_bookRepositoryMock.Object, _categoryRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_AddsBookSuccessfully()
        {
            // Arrange
            var command = new AddBookCommand
            {
                Title = "New Book",
                Author = "New Author",
                PublishedDate = DateTime.Now,
                CategoryNames = new List<string> { "Category 1" }
            };

            var categories = new List<Category>
            {
                new Category { CategoryId = Guid.NewGuid(), Name = "Category 1" }
            };

            var book = new Book
            {
                BookId = Guid.NewGuid(),
                Title = command.Title,
                Author = command.Author,
                PublishedDate = command.PublishedDate,
                BookCategories = categories.Select(c => new BookCategory { Category = c }).ToList()
            };

            var bookDTO = new BookDTO
            {
                BookId = book.BookId,
                Title = book.Title,
                Author = book.Author,
                PublishedDate = book.PublishedDate,
                CategoryNames = command.CategoryNames
            };

            _categoryRepositoryMock.Setup(r => r.GetAllCategoriesAsync()).ReturnsAsync(categories);
            _bookRepositoryMock.Setup(r => r.AddBookAsync(It.IsAny<Book>())).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<BookDTO>(It.IsAny<Book>())).Returns(bookDTO);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _bookRepositoryMock.Verify(r => r.AddBookAsync(It.IsAny<Book>()), Times.Once);
            Assert.Equal(bookDTO.BookId, result.BookId);
            Assert.Equal(bookDTO.Title, result.Title);
        }

        [Fact]
        public async Task Handle_ThrowsException_OnRepositoryError()
        {
            // Arrange
            var command = new AddBookCommand
            {
                Title = "New Book",
                Author = "New Author",
                PublishedDate = DateTime.Now,
                CategoryNames = new List<string> { "Category 1" }
            };

            _categoryRepositoryMock.Setup(r => r.GetAllCategoriesAsync()).ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
