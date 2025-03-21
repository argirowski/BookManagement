﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistence;

#nullable disable

namespace Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Book", b =>
                {
                    b.Property<Guid>("BookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PublishedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BookId");

                    b.ToTable("Books");

                    b.HasData(
                        new
                        {
                            BookId = new Guid("f5d9c3e3-3e6d-6d0c-0d0c-3e6d6d0c0d0c"),
                            Author = "Nathaniel Cross",
                            PublishedDate = new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Embers of the Eternal Flame"
                        },
                        new
                        {
                            BookId = new Guid("a6e0d4f4-4f7e-7e1d-1d1d-4f7e7e1d1d1d"),
                            Author = "Penelope Sinclair",
                            PublishedDate = new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "The Vanishing Hour"
                        });
                });

            modelBuilder.Entity("Domain.Entities.BookCategory", b =>
                {
                    b.Property<Guid>("BookId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("BookId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("BookCategories");

                    b.HasData(
                        new
                        {
                            BookId = new Guid("f5d9c3e3-3e6d-6d0c-0d0c-3e6d6d0c0d0c"),
                            CategoryId = new Guid("ae18653f-848e-4e0c-9942-a3771683c6ec")
                        },
                        new
                        {
                            BookId = new Guid("a6e0d4f4-4f7e-7e1d-1d1d-4f7e7e1d1d1d"),
                            CategoryId = new Guid("82a745d1-ee17-43cc-951d-cd3cbc6d0091")
                        });
                });

            modelBuilder.Entity("Domain.Entities.Category", b =>
                {
                    b.Property<Guid>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            CategoryId = new Guid("ae18653f-848e-4e0c-9942-a3771683c6ec"),
                            Name = "Fiction"
                        },
                        new
                        {
                            CategoryId = new Guid("82a745d1-ee17-43cc-951d-cd3cbc6d0091"),
                            Name = "Non-Fiction"
                        },
                        new
                        {
                            CategoryId = new Guid("af2a4c0a-341b-40b4-ba3f-3d96d969eaa7"),
                            Name = "Travel"
                        },
                        new
                        {
                            CategoryId = new Guid("565d2f77-56c1-4848-b081-05bff5a0df5e"),
                            Name = "Technology"
                        },
                        new
                        {
                            CategoryId = new Guid("3b6b9918-5a55-40f6-98d6-53cfe7185177"),
                            Name = "Philosophy"
                        },
                        new
                        {
                            CategoryId = new Guid("bfc5c985-505e-4cf8-958e-a83473c3a570"),
                            Name = "Psychology"
                        },
                        new
                        {
                            CategoryId = new Guid("f15fc789-4c64-4aa1-b2d9-678bd1750ab3"),
                            Name = "History"
                        },
                        new
                        {
                            CategoryId = new Guid("003d691a-1706-481b-91c7-e8c76e25ad5a"),
                            Name = "Biography"
                        },
                        new
                        {
                            CategoryId = new Guid("5648db86-453a-4dc6-8862-3faa602e245c"),
                            Name = "Western"
                        },
                        new
                        {
                            CategoryId = new Guid("89f157c1-1a76-44c8-8b35-c9dcc75a4ff4"),
                            Name = "Romance"
                        });
                });

            modelBuilder.Entity("Domain.Entities.BookCategory", b =>
                {
                    b.HasOne("Domain.Entities.Book", "Book")
                        .WithMany("BookCategories")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Category", "Category")
                        .WithMany("BookCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Domain.Entities.Book", b =>
                {
                    b.Navigation("BookCategories");
                });

            modelBuilder.Entity("Domain.Entities.Category", b =>
                {
                    b.Navigation("BookCategories");
                });
#pragma warning restore 612, 618
        }
    }
}
