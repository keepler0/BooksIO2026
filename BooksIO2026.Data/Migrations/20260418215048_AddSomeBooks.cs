using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BooksIO2026.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSomeBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookId", "AuthorId", "IsActive", "Price", "PublishedDate", "PublisherId", "Title" },
                values: new object[,]
                {
                    { 1, 1013, true, 50m, new DateTime(2008, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, "Clean Code: A Handbook of Agile Software Craftsmanship" },
                    { 2, 1014, true, 40m, new DateTime(2008, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, "C# in Depth 1st edition" },
                    { 3, 1014, true, 45m, new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, "C# in Depth 2st edition" },
                    { 4, 1013, true, 35m, new DateTime(2017, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, "Clean Architecture: A Craftsman's Guide to Software Structure and Design" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 4);
        }
    }
}
