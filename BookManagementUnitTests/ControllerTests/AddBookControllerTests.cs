using API.Controllers;
using Application.DTOs;
using Application.Features.Commands.AddBook;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BookManagementUnitTests.ControllerTests
{
    public class AddBookControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly BooksController _controller;

        public AddBookControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new BooksController(_mediatorMock.Object);
        }

        [Fact]
        public async Task AddBook_ReturnsCreatedAtActionResult_WithBook()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var book = new BookDTO { BookId = bookId, Title = "Book 1", Author = "Author 1", PublishedDate = DateTime.Now, CategoryNames = new List<string> { "Horror" } };
            _mediatorMock.Setup(m => m.Send(It.IsAny<AddBookCommand>(), default)).ReturnsAsync(book);

            // Act
            var result = await _controller.AddBook(new AddBookCommand { Title = "Book 1", Author = "Author 1", PublishedDate = DateTime.Now, CategoryNames = new List<string> { "Horror" } });

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<BookDTO>(createdAtActionResult.Value);
            Assert.Equal(bookId, returnValue.BookId);
        }
    }
}
