using API.Controllers;
using Application.DTOs;
using Application.Features.Commands.UpdateBook;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BookManagementUnitTests.ControllerTests
{
    public class UpdateBookControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly BooksController _controller;

        public UpdateBookControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new BooksController(_mediatorMock.Object);
        }

        [Fact]
        public async Task UpdateBook_ReturnsOkResult_WithUpdatedBook()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var book = new BookDTO { BookId = bookId, Title = "Updated Book", Author = "Updated Author", PublishedDate = DateTime.Now, CategoryNames = new List<string> { "Updated Category" } };
            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateBookCommand>(), default)).ReturnsAsync(book);

            // Act
            var result = await _controller.UpdateBook(bookId, new UpdateBookCommand { BookId = bookId, Title = "Updated Book", Author = "Updated Author", PublishedDate = DateTime.Now, CategoryNames = new List<string> { "Updated Category" } });

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<BookDTO>(okResult.Value);
            Assert.Equal(bookId, returnValue.BookId);
        }
    }
}
