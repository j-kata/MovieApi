using Microsoft.EntityFrameworkCore;
using MovieApi.Models.Entities;

namespace MovieApi.Data
{
    public class MovieContext(DbContextOptions<MovieContext> options) : DbContext(options)
    {
        public DbSet<Genre> Genres { get; set; } = default!;
        public DbSet<Movie> Movies { get; set; } = default!;
        public DbSet<Review> Reviews { get; set; } = default!;
        public DbSet<Actor> Actors { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Genre)
                .WithMany(g => g.Movies)
                .HasForeignKey(m => m.GenreId);

            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Actors)
                .WithMany(a => a.Movies)
                .UsingEntity(j => j.ToTable("MovieActor")); // Many-to-many relationship

            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Reviews)
                .WithOne(r => r.Movie)
                .HasForeignKey(r => r.MovieId);

            modelBuilder.Entity<Movie>()
                .HasOne(m => m.MovieDetails)
                .WithOne(d => d.Movie)
                .HasForeignKey<Movie>(m => m.MovieDetailsId);
        }
    }
}
