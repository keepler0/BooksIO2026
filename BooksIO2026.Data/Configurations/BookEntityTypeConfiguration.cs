using BooksIO2026.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BooksIO2026.Data.Configurations
{
    public class BookEntityTypeConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasIndex(b => b.Title)
                   .IsUnique()
                   .HasDatabaseName("IX_Books_Title");

            builder.Property(b => b.Title)
                   .IsRequired()
                   .HasMaxLength(100);
            //TODO: Aplicar mas configuraciones Fijarse en WHATSAPP
        }
    }
}
