using Microsoft.EntityFrameworkCore;
using MovieApp.Core.Contracts;
using MovieApp.Core.ValueObjects;
using MovieApp.Data.Extensions;

namespace MovieApp.Data.Repositories;

public class ReportRepository(MovieContext context) : IReportRepository
{
    public async Task<IEnumerable<TopMoviesByGenre>> GetTopMoviesByGenreAsync(int topCount, bool trackChanges = false) =>
        await context.Movies
            .WithTracking(trackChanges)
            .Where(m => m.Reviews.Any())
            .GroupBy(m => new { m.Genre.Id, m.Genre.Name })
            .Select(g => new TopMoviesByGenre
            {
                Genre = g.Key.Name,
                Movies = g.Select(m => new MovieWithRating
                {
                    Id = m.Id,
                    Title = m.Title,
                    Rating = m.Reviews.Average(r => r.Rating)
                })
                .OrderByDescending(m => m.Rating)
                .Take(topCount)
            })
            .ToListAsync();

    public async Task<IEnumerable<GenreWithRating>> GetGenresAvgRatingAsync(bool trackChanges = false) =>
        await context.Reviews
            .WithTracking(trackChanges)
            .GroupBy(r => new { r.Movie.Genre.Id, r.Movie.Genre.Name })
            .Select(g => new GenreWithRating
            {
                Id = g.Key.Id,
                Genre = g.Key.Name,
                Rating = g.Average(g => g.Rating)
            })
            .ToListAsync();

    public async Task<ActorWithRolesCount?> GetActorWithMostRolesAsync(bool trackChanges = false) =>
        await context.Actors
            .WithTracking(trackChanges)
            .Select(a => new ActorWithRolesCount
            {
                Id = a.Id,
                Name = a.Name,
                RolesCount = a.Roles.Count
            })
            .OrderByDescending(a => a.RolesCount)
            .FirstOrDefaultAsync();

    public async Task<MovieWithReviewsCount?> GetFilmWithMostReviewsAsync(bool trackChanges = false) =>
        await context.Movies
            .WithTracking(trackChanges)
            .Select(m => new MovieWithReviewsCount
            {
                Id = m.Id,
                Title = m.Title,
                ReviewsCount = m.Reviews.Count
            })
            .OrderByDescending(m => m.ReviewsCount)
            .FirstOrDefaultAsync();

    public async Task<GenreWithMoviesCount?> GetGenreWithMostMoviesAsync(bool trackChanges = false) =>
        await context.Genres
            .WithTracking(trackChanges)
            .Select(g => new GenreWithMoviesCount
            {
                Name = g.Name,
                MoviesCount = g.Movies.Count
            })
            .OrderByDescending(m => m.MoviesCount)
            .FirstOrDefaultAsync();
}