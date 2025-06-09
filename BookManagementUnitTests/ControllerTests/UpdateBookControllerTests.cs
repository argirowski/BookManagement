using API.Controllers;
using Application.DTOs;
using Application.Features.Commands.UpdateBook;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using AutoMapper;

namespace BookManagementUnitTests.ControllerTests
{
    public class UpdateBookControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly BooksController _controller;

        public UpdateBookControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _mapperMock = new Mock<IMapper>();
            _controller = new BooksController(_mediatorMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task UpdateBook_ReturnsOkResult_WithUpdatedBook()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var book = new BookDTO { BookId = bookId, Title = "Updated Book", Author = "Updated Author", PublishedDate = DateTime.Now, CategoryNames = new List<string> { "Updated Category" } };
            var dto = new UpdateBookDTO { Title = "Updated Book", Author = "Updated Author", PublishedDate = DateTime.Now, CategoryNames = new List<string> { "Updated Category" } };
            var command = new UpdateBookCommand { BookId = bookId, Title = dto.Title, Author = dto.Author, PublishedDate = dto.PublishedDate, CategoryNames = dto.CategoryNames };
            _mapperMock.Setup(m => m.Map<UpdateBookCommand>(dto)).Returns(command);
            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateBookCommand>(), default)).ReturnsAsync(book);

            // Act
            var result = await _controller.UpdateBook(bookId, dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<BookDTO>(okResult.Value);
            Assert.Equal(bookId, returnValue.BookId);
        }
    }
}
