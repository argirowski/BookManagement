using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddingAnotherMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: new Guid("a6e0d4f4-4f7e-7e1d-1d1d-4f7e7e1d1d1d"),
                columns: new[] { "Author", "Title" },
                values: new object[] { "Penelope Sinclair", "The Vanishing Hour" });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: new Guid("f5d9c3e3-3e6d-6d0c-0d0c-3e6d6d0c0d0c"),
                columns: new[] { "Author", "Title" },
                values: new object[] { "Nathaniel Cross", "Embers of the Eternal Flame" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: new Guid("a6e0d4f4-4f7e-7e1d-1d1d-4f7e7e1d1d1d"),
                columns: new[] { "Author", "Title" },
                values: new object[] { "Author 2", "Book 2" });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: new Guid("f5d9c3e3-3e6d-6d0c-0d0c-3e6d6d0c0d0c"),
                columns: new[] { "Author", "Title" },
                values: new object[] { "Author 1", "Book 1" });
        }
    }
}
