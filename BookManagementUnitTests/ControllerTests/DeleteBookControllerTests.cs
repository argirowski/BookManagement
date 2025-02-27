using API.Controllers;
using Application.Features.Commands.DeleteBook;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BookManagementUnitTests.ControllerTests
{
    public class DeleteBookControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly BooksController _controller;

        public DeleteBookControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new BooksController(_mediatorMock.Object);
        }

        [Fact]
        public async Task DeleteBook_ReturnsNoContentResult()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteBookCommand>(), default)).ReturnsAsync(Unit.Value);

            // Act
            var result = await _controller.DeleteBook(bookId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
