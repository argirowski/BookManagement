using Application.Features.Commands.UpdateBook;
using Application.Validators;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation.TestHelper;
using Moq;

namespace BookManagementUnitTests.ValidatorTests
{
    public class UpdateBookCommandValidatorTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly UpdateBookCommandValidator _validator;

        public UpdateBookCommandValidatorTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _validator = new UpdateBookCommandValidator(_bookRepositoryMock.Object, _categoryRepositoryMock.Object);
        }

        [Fact]
        public void Should_Have_Error_When_BookId_Is_Empty()
        {
            // Arrange
            var command = new UpdateBookCommand
            {
                BookId = Guid.Empty,
                Title = "Title",
                Author = "Author",
                PublishedDate = DateTime.Now,
                CategoryNames = new List<string> { "Category1" }
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.BookId);
        }

        [Fact]
        public void Should_Have_Error_When_Title_Is_Empty()
        {
            // Arrange
            var command = new UpdateBookCommand
            {
                BookId = Guid.NewGuid(),
                Title = string.Empty,
                Author = "Author",
                PublishedDate = DateTime.Now,
                CategoryNames = new List<string> { "Category1" }
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Title);
        }

        [Fact]
        public void Should_Have_Error_When_Author_Is_Empty()
        {
            // Arrange
            var command = new UpdateBookCommand
            {
                BookId = Guid.NewGuid(),
                Title = "Title",
                Author = string.Empty,
                PublishedDate = DateTime.Now,
                CategoryNames = new List<string> { "Category1" }
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Author);
        }

        [Fact]
        public void Should_Have_Error_When_PublishedDate_Is_Empty()
        {
            // Arrange
            var command = new UpdateBookCommand
            {
                BookId = Guid.NewGuid(),
                Title = "Title",
                Author = "Author",
                PublishedDate = DateTime.MinValue,
                CategoryNames = new List<string> { "Category1" }
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PublishedDate);
        }

        [Fact]
        public void Should_Have_Error_When_CategoryNames_Is_Empty()
        {
            // Arrange
            var command = new UpdateBookCommand
            {
                BookId = Guid.NewGuid(),
                Title = "Title",
                Author = "Author",
                PublishedDate = DateTime.Now,
                CategoryNames = new List<string>()
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CategoryNames);
        }

        [Fact]
        public void Should_Have_Error_When_Book_Does_Not_Exist()
        {
            // Arrange
            _bookRepositoryMock.Setup(repo => repo.GetBookByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Book?)null);

            var command = new UpdateBookCommand
            {
                BookId = Guid.NewGuid(),
                Title = "Title",
                Author = "Author",
                PublishedDate = DateTime.Now,
                CategoryNames = new List<string> { "Category1" }
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.BookId).WithErrorMessage("The specified book does not exist.");
        }

        [Fact]
        public void Should_Have_Error_When_Categories_Do_Not_Exist()
        {
            // Arrange
            _categoryRepositoryMock.Setup(repo => repo.GetAllCategoriesAsync()).ReturnsAsync(new List<Category>());
            var command = new UpdateBookCommand
            {
                BookId = Guid.NewGuid(),
                Title = "Title",
                Author = "Author",
                PublishedDate = DateTime.Now,
                CategoryNames = new List<string> { "NonExistentCategory" }
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CategoryNames).WithErrorMessage("One or more categories do not exist.");
        }
    }
}
