using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BooksIO2026.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewRecordsToAuthorsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Authors (FirstName, LastName) VALUES ('John', 'Doe')");
            migrationBuilder.Sql("INSERT INTO Authors (FirstName, LastName) VALUES ('Jane', 'Austen')");
            migrationBuilder.Sql("INSERT INTO Authors (FirstName, LastName) VALUES ('Isaac', 'Asimov')");
            migrationBuilder.Sql("INSERT INTO Authors (FirstName, LastName) VALUES ('Ursula', 'Le Guin')");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Authors");
        }
    }
}
