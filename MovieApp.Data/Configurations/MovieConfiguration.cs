using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieApp.Core.Entities;

namespace MovieApp.Data.Configurations
{
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.ToTable("Movies");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Title)
                .HasMaxLength(200)
                .IsRequired();

            builder.HasIndex(m => m.Title)
                .IsUnique();

            builder.Property(m => m.Year)
                .IsRequired();

            builder.Property(m => m.Duration)
                .IsRequired();

            builder.HasIndex(m => m.GenreId);

            builder.HasOne(m => m.Genre)
                .WithMany(g => g.Movies)
                .HasForeignKey(m => m.GenreId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}