using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieAPI.Models.Entities;

namespace MovieAPI.Data.Configurations
{
    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.ToTable("Genres");

            builder.HasKey(g => g.Id);

            builder.Property(g => g.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(g => g.Name)
                .IsUnique();
        }
    }
}