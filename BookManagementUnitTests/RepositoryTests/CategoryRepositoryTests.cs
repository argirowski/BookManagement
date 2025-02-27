using Domain.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace BookManagementUnitTests.RepositoryTests
{
    public class CategoryRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly CategoryRepository _repository;

        public CategoryRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new CategoryRepository(_context);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.AddRange(
                    new Category { CategoryId = Guid.NewGuid(), Name = "Category1" },
                    new Category { CategoryId = Guid.NewGuid(), Name = "Category2" }
                );
                _context.SaveChanges();
            }
        }

        [Fact]
        public async Task GetAllCategoriesAsync_Should_Return_All_Categories()
        {
            // Act
            var categories = await _repository.GetAllCategoriesAsync();

            // Assert
            Assert.Equal(2, categories.Count());
        }

        [Fact]
        public async Task GetCategoryByNameAsync_Should_Return_Correct_Category()
        {
            // Act
            var category = await _repository.GetCategoryByNameAsync("Category1");

            // Assert
            Assert.NotNull(category);
            Assert.Equal("Category1", category.Name);
        }

        [Fact]
        public async Task GetCategoryByNameAsync_Should_Return_Null_If_Category_Does_Not_Exist()
        {
            // Act
            var category = await _repository.GetCategoryByNameAsync("NonExistentCategory");

            // Assert
            Assert.Null(category);
        }
    }
}