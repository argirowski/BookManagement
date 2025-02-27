using API.Controllers;
using Application.DTOs;
using Application.Features.Queries.GetAllBooks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BookManagementUnitTests.ControllerTests
{
    public class GetAllBooksControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly BooksController _controller;

        public GetAllBooksControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new BooksController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetAllBooks_ReturnsOkResult_WithListOfBooks()
        {
            // Arrange
            var books = new List<BookDTO>
            {
                new BookDTO { BookId = Guid.NewGuid(), Title = "Book 1", Author = "Author 1", PublishedDate = DateTime.Now, CategoryNames = new List<string> { "Category 1" } },
                new BookDTO { BookId = Guid.NewGuid(), Title = "Book 2", Author = "Author 2", PublishedDate = DateTime.Now, CategoryNames = new List<string> { "Category 2" } }
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllBooksQuery>(), default)).ReturnsAsync(books);

            // Act
            var result = await _controller.GetAllBooks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<BookDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetAllBooks_ReturnsOkResult_WithEmptyList()
        {
            // Arrange
            var books = new List<BookDTO>();
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllBooksQuery>(), default)).ReturnsAsync(books);

            // Act
            var result = await _controller.GetAllBooks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<BookDTO>>(okResult.Value);
            Assert.Empty(returnValue);
        }
    }
}
