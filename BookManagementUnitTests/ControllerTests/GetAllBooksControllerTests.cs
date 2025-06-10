using API.Controllers;
using Application.DTOs;
using Application.Features.Queries.GetAllBooks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using AutoMapper;
using Application.Helpers;

namespace BookManagementUnitTests.ControllerTests
{
    public class GetAllBooksControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly BooksController _controller;

        public GetAllBooksControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _mapperMock = new Mock<IMapper>();
            _controller = new BooksController(_mediatorMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllBooks_ReturnsOkResult_WithPagedResult()
        {
            // Arrange
            var pagedResult = new PagedResultDTO<BookDTO>
            {
                Items = new List<BookDTO>
                {
                    new BookDTO { BookId = Guid.NewGuid(), Title = "Book 1", Author = "Author 1", PublishedDate = DateTime.Now, CategoryNames = new List<string> { "Category 1" } },
                    new BookDTO { BookId = Guid.NewGuid(), Title = "Book 2", Author = "Author 2", PublishedDate = DateTime.Now, CategoryNames = new List<string> { "Category 2" } }
                },
                TotalCount = 2,
                Page = 1,
                PageSize = 5,
                // TotalPages is calculated
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllBooksQuery>(), default)).ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.GetAllBooks(new PaginationParams { PageNumber = 1, PageSize = 5 });

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<PagedResultDTO<BookDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Items.Count);
            Assert.Equal(1, returnValue.Page);
            Assert.Equal(5, returnValue.PageSize);
        }

        [Fact]
        public async Task GetAllBooks_ReturnsOkResult_WithEmptyPagedResult()
        {
            // Arrange
            var pagedResult = new PagedResultDTO<BookDTO>
            {
                Items = new List<BookDTO>(),
                TotalCount = 0,
                Page = 1,
                PageSize = 5
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllBooksQuery>(), default)).ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.GetAllBooks(new PaginationParams { PageNumber = 1, PageSize = 5 });

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<PagedResultDTO<BookDTO>>(okResult.Value);
            Assert.Empty(returnValue.Items);
            Assert.Equal(1, returnValue.Page);
            Assert.Equal(5, returnValue.PageSize);
        }
    }
}
