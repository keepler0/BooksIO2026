using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BooksIO2026.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewRecordsToPublisherTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"INSERT INTO Publishers (Name,Country,FoundedDate,Email,IsActive) 
                                   VALUES ('Harmony Press','Suiza','1940-05-24','harmonypress@gmail.com',1)");

            migrationBuilder.Sql(@"INSERT INTO Publishers (Name,Country,FoundedDate,Email,IsActive) 
                                   VALUES ('PulseWave Media','Estados Unidos','1980-12-03','pulseWave@outlook.com',0)");

            migrationBuilder.Sql(@"INSERT INTO Publishers (Name,Country,FoundedDate,Email,IsActive) 
                                   VALUES ('Tinta sonora','Argentina','1995-10-02','tintaSonora@hotmail.com',1)");

            migrationBuilder.Sql(@"INSERT INTO Publishers (Name,Country,FoundedDate,Email,IsActive) 
                                   VALUES ('Editorial Ritmo vivo','Argentina','1968-02-22','editorial-ritmovivo@yahoo.com',1)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Publishers");
        }
    }
}
