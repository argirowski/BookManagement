using Application.Features.Queries.GetBookById;
using Application.Validators;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation.TestHelper;
using Moq;

namespace BookManagementUnitTests.ValidatorTests
{
    public class GetBookByIdQueryValidatorTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly GetBookByIdQueryValidator _validator;

        public GetBookByIdQueryValidatorTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _validator = new GetBookByIdQueryValidator(_bookRepositoryMock.Object);
        }

        [Fact]
        public void Should_Have_Error_When_BookId_Is_Empty()
        {
            // Arrange
            var query = new GetBookByIdQuery(Guid.Empty);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.BookId);
        }

        [Fact]
        public async Task Should_Have_Error_When_Book_Does_Not_Exist()
        {
            // Arrange
            _bookRepositoryMock.Setup(repo => repo.GetBookByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Book?)null);
            var query = new GetBookByIdQuery(Guid.NewGuid());

            // Act
            var result = await _validator.TestValidateAsync(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.BookId).WithErrorMessage("The specified book does not exist.");
        }

        [Fact]
        public async Task Should_Not_Have_Error_When_Book_Exists()
        {
            // Arrange
            var existingBook = new Book { BookId = Guid.NewGuid() };
            _bookRepositoryMock.Setup(repo => repo.GetBookByIdAsync(It.IsAny<Guid>())).ReturnsAsync(existingBook);
            var query = new GetBookByIdQuery(existingBook.BookId);

            // Act
            var result = await _validator.TestValidateAsync(query);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.BookId);
        }
    }
}
