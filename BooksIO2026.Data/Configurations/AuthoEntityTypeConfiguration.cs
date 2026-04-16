using BooksIO2026.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BooksIO2026.Data.Configurations
{
    public class AuthoEntityTypeConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            //Aqui configuramos la entidad Author,aplicando validaciones y restricciones en sus propiedades
            //builder.HasKey(a => a.AuthorId);
            builder.HasIndex(a => new { a.FirstName, a.LastName }).IsUnique().HasDatabaseName("IX_Authors_FirstName_LastName");
            builder.Property(a => a.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(a => a.LastName).IsRequired().HasMaxLength(50);
        }
    }
}
