using Application.Features.Commands.AddBook;
using Application.Validators;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation.TestHelper;
using Moq;

namespace Application.UnitTests.Validators
{
    public class AddBookCommandValidatorTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly AddBookCommandValidator _validator;

        public AddBookCommandValidatorTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _validator = new AddBookCommandValidator(_bookRepositoryMock.Object, _categoryRepositoryMock.Object);
        }

        [Fact]
        public void Should_Have_Error_When_Title_Is_Empty()
        {
            // Arrange
            var command = new AddBookCommand
            {
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
        public void Should_Have_Error_When_Title_Already_Exists()
        {
            // Arrange
            _bookRepositoryMock.Setup(repo => repo.GetAllBooksAsync()).ReturnsAsync(new List<Book>
            {
                new Book { Title = "Existing Title" }
            });
            // Arrange
            var command = new AddBookCommand
            {
                Title = "Existing Title",
                Author = "Author",
                PublishedDate = DateTime.Now,
                CategoryNames = new List<string> { "Category1" }
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Title).WithErrorMessage("A book with the same title already exists.");
        }

        [Fact]
        public void Should_Have_Error_When_Author_Is_Empty()
        {
            // Arrange
            var command = new AddBookCommand
            {
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
            var command = new AddBookCommand
            {
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
            var command = new AddBookCommand { CategoryNames = new List<string>() };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CategoryNames);
        }

        [Fact]
        public void Should_Have_Error_When_Categories_Do_Not_Exist()
        {
            // Arrange
            _categoryRepositoryMock.Setup(repo => repo.GetAllCategoriesAsync()).ReturnsAsync(new List<Category>());
            var command = new AddBookCommand { CategoryNames = new List<string> { "NonExistentCategory" } };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CategoryNames).WithErrorMessage("One or more categories do not exist.");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Command_Is_Valid()
        {
            // Arrange
            _bookRepositoryMock.Setup(repo => repo.GetAllBooksAsync()).ReturnsAsync(new List<Book>());
            _categoryRepositoryMock.Setup(repo => repo.GetAllCategoriesAsync()).ReturnsAsync(new List<Category>
            {
                new Category { Name = "Category1" }
            });
            var command = new AddBookCommand
            {
                Title = "Valid Title",
                Author = "Valid Author",
                PublishedDate = new DateTime(2024, 1, 1),
                CategoryNames = new List<string> { "Category1" }
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}