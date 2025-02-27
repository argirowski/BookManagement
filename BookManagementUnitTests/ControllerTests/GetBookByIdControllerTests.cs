using API.Controllers;
using Application.DTOs;
using Application.Features.Queries.GetBookById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BookManagementUnitTests.ControllerTests
{
    public class GetBookByIdControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly BooksController _controller;

        public GetBookByIdControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new BooksController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetBookById_ReturnsOkResult_WithBook()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var book = new BookDTO { BookId = bookId, Title = "Book 1", Author = "Author 1", PublishedDate = DateTime.Now, CategoryNames = new List<string> { "Category 1" } };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetBookByIdQuery>(), default)).ReturnsAsync(book);

            // Act
            var result = await _controller.GetBookById(bookId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<BookDTO>(okResult.Value);
            Assert.Equal(bookId, returnValue.BookId);
        }
    }
}
