using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddingMoreCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublishedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookId);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "BookCategories",
                columns: table => new
                {
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookCategories", x => new { x.BookId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_BookCategories_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookId", "Author", "PublishedDate", "Title" },
                values: new object[,]
                {
                    { new Guid("a6e0d4f4-4f7e-7e1d-1d1d-4f7e7e1d1d1d"), "Author 2", new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Book 2" },
                    { new Guid("f5d9c3e3-3e6d-6d0c-0d0c-3e6d6d0c0d0c"), "Author 1", new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Book 1" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Name" },
                values: new object[,]
                {
                    { new Guid("003d691a-1706-481b-91c7-e8c76e25ad5a"), "Biography" },
                    { new Guid("3b6b9918-5a55-40f6-98d6-53cfe7185177"), "Philosophy" },
                    { new Guid("5648db86-453a-4dc6-8862-3faa602e245c"), "Western" },
                    { new Guid("565d2f77-56c1-4848-b081-05bff5a0df5e"), "Technology" },
                    { new Guid("82a745d1-ee17-43cc-951d-cd3cbc6d0091"), "Non-Fiction" },
                    { new Guid("89f157c1-1a76-44c8-8b35-c9dcc75a4ff4"), "Romance" },
                    { new Guid("ae18653f-848e-4e0c-9942-a3771683c6ec"), "Fiction" },
                    { new Guid("af2a4c0a-341b-40b4-ba3f-3d96d969eaa7"), "Travel" },
                    { new Guid("bfc5c985-505e-4cf8-958e-a83473c3a570"), "Psychology" },
                    { new Guid("f15fc789-4c64-4aa1-b2d9-678bd1750ab3"), "History" }
                });

            migrationBuilder.InsertData(
                table: "BookCategories",
                columns: new[] { "BookId", "CategoryId" },
                values: new object[,]
                {
                    { new Guid("a6e0d4f4-4f7e-7e1d-1d1d-4f7e7e1d1d1d"), new Guid("82a745d1-ee17-43cc-951d-cd3cbc6d0091") },
                    { new Guid("f5d9c3e3-3e6d-6d0c-0d0c-3e6d6d0c0d0c"), new Guid("ae18653f-848e-4e0c-9942-a3771683c6ec") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookCategories_CategoryId",
                table: "BookCategories",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookCategories");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
