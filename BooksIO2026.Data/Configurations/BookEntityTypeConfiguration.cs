using BooksIO2026.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BooksIO2026.Data.Configurations
{
    public class BookEntityTypeConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasIndex(b => b.Title);

            builder.HasIndex(b => new { b.Title, b.AuthorId })
                   .IsUnique();

            builder.Property(b => b.Title)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(b => b.Price)
                   .IsRequired()
                   .HasColumnType("decimal(10,2)");

            builder.Property(b => b.PublishedDate)
                   .IsRequired();

            builder.Property(b => b.IsActive)
                   .IsRequired();

            //Declaramos la relacion de 1 a muchos entre Books y Authors
            builder.HasOne(b => b.Author)
                   .WithMany(a => a.Books)
                   .HasForeignKey(b => b.AuthorId)
                   .OnDelete(DeleteBehavior.Restrict);

            //declaramos la relacion de 1 a muchos entre books y Publisher
            builder.HasOne(b => b.Publisher)
                   .WithMany(p => p.Books)
                   .HasForeignKey(b => b.PublisherId)
                   .OnDelete(DeleteBehavior.Restrict);

            #region Hardcoded records

            builder.HasData(new List<Book>()
            {
                new Book
                {
                    BookId = 1,
                    Title="Clean Code: A Handbook of Agile Software Craftsmanship",
                    PublishedDate=new DateTime(2008,01,08),
                    Price=50,
                    IsActive=true,
                    AuthorId=1013,
                    PublisherId=7
                },
                new Book
                {
                    BookId = 2,
                    Title="C# in Depth 1st edition",
                    PublishedDate=new DateTime(2008,01,01),
                    Price=40,
                    IsActive=true,
                    AuthorId=1014,
                    PublisherId=9
                },
                new Book
                {
                    BookId = 3,
                    Title="C# in Depth 2st edition",
                    PublishedDate=new DateTime(2010,01,01),
                    Price=45,
                    IsActive=true,
                    AuthorId=1014,
                    PublisherId=9
                },
                new Book
                {
                    BookId = 4,
                    Title="Clean Architecture: A Craftsman's Guide to Software Structure and Design",
                    PublishedDate=new DateTime(2017,09,20),
                    Price=35,
                    IsActive=true,
                    AuthorId=1013,
                    PublisherId=7
                }
            });

            #endregion
        }
    }
}
