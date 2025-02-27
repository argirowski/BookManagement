using Application.Features.Commands.DeleteBook;
using Application.Validators;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation.TestHelper;
using Moq;

namespace BookManagementUnitTests.ValidatorTests
{
    public class DeleteBookCommandValidatorTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly DeleteBookCommandValidator _validator;

        public DeleteBookCommandValidatorTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _validator = new DeleteBookCommandValidator(_bookRepositoryMock.Object);
        }

        [Fact]
        public void Should_Have_Error_When_BookId_Is_Empty()
        {
            // Arrange
            var command = new DeleteBookCommand(Guid.Empty);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.BookId);
        }

        [Fact]
        public void Should_Have_Error_When_Book_Does_Not_Exist()
        {
            // Arrange
            _bookRepositoryMock.Setup(repo => repo.GetBookByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Book?)null);
            var command = new DeleteBookCommand(Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.BookId).WithErrorMessage("The specified book does not exist.");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Book_Exists()
        {
            // Arrange
            var existingBook = new Book { BookId = Guid.NewGuid() };
            _bookRepositoryMock.Setup(repo => repo.GetBookByIdAsync(It.IsAny<Guid>())).ReturnsAsync(existingBook);
            var command = new DeleteBookCommand(existingBook.BookId);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.BookId);
        }
    }
}
