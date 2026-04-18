using BooksIO2026.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BooksIO2026.Data.Configurations
{
    public class PublisherEntityTypeConfiguration : IEntityTypeConfiguration<Publisher>
    {
        public void Configure(EntityTypeBuilder<Publisher> builder)
        {
            builder.HasIndex(p => new { p.Name, p.Country })
                   .IsUnique()
                   .HasDatabaseName("IX_Publishers_Name_Country");

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.Country)
                   .IsRequired()
                   .HasMaxLength(60);
        }
    }
}