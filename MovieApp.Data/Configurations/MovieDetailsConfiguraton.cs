using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieApp.Core.Entities;

namespace MovieApp.Data.Configurations;

public class MovieDetailConfiguration : IEntityTypeConfiguration<MovieDetail>
{
    public void Configure(EntityTypeBuilder<MovieDetail> builder)
    {
        builder.ToTable("MovieDetails");

        builder.HasKey(md => md.Id);

        builder.Property(md => md.Synopsis)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(md => md.Language)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(md => md.Budget)
            .IsRequired(false);

        builder.HasIndex(md => md.MovieId)
            .IsUnique();

        builder.HasOne(md => md.Movie)
            .WithOne(m => m.MovieDetail)
            .HasForeignKey<MovieDetail>(md => md.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
