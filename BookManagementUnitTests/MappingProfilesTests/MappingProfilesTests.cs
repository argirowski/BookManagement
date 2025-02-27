using Application.DTOs;
using Application.Mapping;
using AutoMapper;
using Domain.Entities;

namespace BookManagementUnitTests.MappingProfilesTests
{
    public class MappingProfilesTests
    {
        private readonly IMapper _mapper;

        public MappingProfilesTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfiles>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void Should_Map_Book_To_BookDTO()
        {
            // Arrange
            var book = new Book
            {
                BookId = Guid.NewGuid(),
                Title = "Test Title",
                Author = "Test Author",
                PublishedDate = DateTime.Now,
                BookCategories = new List<BookCategory>
                {
                    new BookCategory { Category = new Category { Name = "Category1" } },
                    new BookCategory { Category = new Category { Name = "Category2" } }
                }
            };

            // Act
            var bookDTO = _mapper.Map<BookDTO>(book);

            // Assert
            Assert.Equal(book.BookId, bookDTO.BookId);
            Assert.Equal(book.Title, bookDTO.Title);
            Assert.Equal(book.Author, bookDTO.Author);
            Assert.Equal(book.PublishedDate, bookDTO.PublishedDate);
            Assert.Equal(book.BookCategories.Select(bc => bc.Category.Name).ToList(), bookDTO.CategoryNames);
        }

        [Fact]
        public void Should_Map_BookDTO_To_Book()
        {
            // Arrange
            var bookDTO = new BookDTO
            {
                BookId = Guid.NewGuid(),
                Title = "Test Title",
                Author = "Test Author",
                PublishedDate = DateTime.Now,
                CategoryNames = new List<string> { "Category1", "Category2" }
            };

            // Act
            var book = _mapper.Map<Book>(bookDTO);

            // Assert
            Assert.Equal(bookDTO.BookId, book.BookId);
            Assert.Equal(bookDTO.Title, book.Title);
            Assert.Equal(bookDTO.Author, book.Author);
            Assert.Equal(bookDTO.PublishedDate, book.PublishedDate);
            Assert.Equal(bookDTO.CategoryNames, book.BookCategories.Select(bc => bc.Category.Name).ToList());
        }

        [Fact]
        public void Should_Map_Category_To_CategoryDTO()
        {
            // Arrange
            var category = new Category
            {
                CategoryId = Guid.NewGuid(),
                Name = "Test Category"
            };

            // Act
            var categoryDTO = _mapper.Map<CategoryDTO>(category);

            // Assert
            Assert.Equal(category.CategoryId, categoryDTO.CategoryId);
            Assert.Equal(category.Name, categoryDTO.Name);
        }

        [Fact]
        public void Should_Map_CategoryDTO_To_Category()
        {
            // Arrange
            var categoryDTO = new CategoryDTO
            {
                CategoryId = Guid.NewGuid(),
                Name = "Test Category"
            };

            // Act
            var category = _mapper.Map<Category>(categoryDTO);

            // Assert
            Assert.Equal(categoryDTO.CategoryId, category.CategoryId);
            Assert.Equal(categoryDTO.Name, category.Name);
        }
    }
}