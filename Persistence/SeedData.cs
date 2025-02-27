using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public static class SeedData
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var category1Id = Guid.Parse("ae18653f-848e-4e0c-9942-a3771683c6ec");
            var category2Id = Guid.Parse("82a745d1-ee17-43cc-951d-cd3cbc6d0091");
            var category3Id = Guid.Parse("af2a4c0a-341b-40b4-ba3f-3d96d969eaa7");
            var category4Id = Guid.Parse("565d2f77-56c1-4848-b081-05bff5a0df5e");
            var category5Id = Guid.Parse("3b6b9918-5a55-40f6-98d6-53cfe7185177");
            var category6Id = Guid.Parse("bfc5c985-505e-4cf8-958e-a83473c3a570");
            var category7Id = Guid.Parse("f15fc789-4c64-4aa1-b2d9-678bd1750ab3");
            var category8Id = Guid.Parse("003d691a-1706-481b-91c7-e8c76e25ad5a");
            var category9Id = Guid.Parse("5648db86-453a-4dc6-8862-3faa602e245c");
            var category10Id = Guid.Parse("89f157c1-1a76-44c8-8b35-c9dcc75a4ff4");

            var book1Id = Guid.Parse("f5d9c3e3-3e6d-6d0c-0d0c-3e6d6d0c0d0c");
            var book2Id = Guid.Parse("a6e0d4f4-4f7e-7e1d-1d1d-4f7e7e1d1d1d");

            var category1 = new Category { CategoryId = category1Id, Name = "Fiction" };
            var category2 = new Category { CategoryId = category2Id, Name = "Non-Fiction" };
            var category3 = new Category { CategoryId = category3Id, Name = "Travel" };
            var category4 = new Category { CategoryId = category4Id, Name = "Technology" };
            var category5 = new Category { CategoryId = category5Id, Name = "Philosophy" };
            var category6 = new Category { CategoryId = category6Id, Name = "Psychology" };
            var category7 = new Category { CategoryId = category7Id, Name = "History" };
            var category8 = new Category { CategoryId = category8Id, Name = "Biography" };
            var category9 = new Category { CategoryId = category9Id, Name = "Western" };
            var category10 = new Category { CategoryId = category10Id, Name = "Romance" };

            var book1 = new Book
            {
                BookId = book1Id,
                Title = "Embers of the Eternal Flame",
                Author = "Nathaniel Cross",
                PublishedDate = new DateTime(2020, 1, 1)
            };

            var book2 = new Book
            {
                BookId = book2Id,
                Title = "The Vanishing Hour",
                Author = "Penelope Sinclair",
                PublishedDate = new DateTime(2021, 1, 1)
            };

            modelBuilder.Entity<Category>().HasData(category1, category2, category3, category4, category5, category6, category7, category8, category9, category10);
            modelBuilder.Entity<Book>().HasData(book1, book2);

            modelBuilder.Entity<BookCategory>().HasData(
                new BookCategory { BookId = book1Id, CategoryId = category1Id },
                new BookCategory { BookId = book2Id, CategoryId = category2Id }
            );
        }
    }
}