using API.Controllers;
using Application.DTOs;
using Application.Features.Commands.AddBook;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using AutoMapper;

namespace BookManagementUnitTests.ControllerTests
{
    public class AddBookControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly BooksController _controller;

        public AddBookControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _mapperMock = new Mock<IMapper>();
            _controller = new BooksController(_mediatorMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task AddBook_ReturnsCreatedAtActionResult_WithBook()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var book = new BookDTO { BookId = bookId, Title = "Book 1", Author = "Author 1", PublishedDate = DateTime.Now, CategoryNames = new List<string> { "Horror" } };
            var dto = new AddBookDTO { Title = "Book 1", Author = "Author 1", PublishedDate = DateTime.Now, CategoryNames = new List<string> { "Horror" } };
            var command = new AddBookCommand { Title = dto.Title, Author = dto.Author, PublishedDate = dto.PublishedDate, CategoryNames = dto.CategoryNames };
            _mapperMock.Setup(m => m.Map<AddBookCommand>(dto)).Returns(command);
            _mediatorMock.Setup(m => m.Send(It.IsAny<AddBookCommand>(), default)).ReturnsAsync(book);

            // Act
            var result = await _controller.AddBook(dto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<BookDTO>(createdAtActionResult.Value);
            Assert.Equal(bookId, returnValue.BookId);
        }
    }
}
