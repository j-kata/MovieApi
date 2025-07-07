using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Models.Dtos.Reports;

namespace MovieApi.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportsController(MovieContext context, IMapper mapper)
        : AppController(context, mapper)
    {
        [HttpGet("movies/top5pergenre")]
        public async Task<ActionResult<IEnumerable<TopMoviesByGenreDto>>> GetTopFiveByGenre()
        {
            var report = await _context.Movies
                .Where(m => m.Reviews.Any())
                .GroupBy(m => new { m.Genre.Id, m.Genre.Name })
                .Select(g => new TopMoviesByGenreDto
                {
                    Genre = g.Key.Name,
                    Movies = g.Select(m => new MovieWithRatingDto
                    {
                        Id = m.Id,
                        Title = m.Title,
                        Rating = m.Reviews.Average(r => r.Rating)
                    })
                    .OrderByDescending(m => m.Rating)
                    .Take(5)
                })
                .ToListAsync();

            return Ok(report);
        }

        [HttpGet("movies/average-ratings")]
        public async Task<ActionResult<IEnumerable<GenreWithRatingDto>>> GetRatingsByGenre()
        {
            var report = await _context.Reviews
                .GroupBy(r => new { r.Movie.Genre.Id, r.Movie.Genre.Name })
                .Select(g => new GenreWithRatingDto
                {
                    Genre = g.Key.Name,
                    Rating = g.Average(g => g.Rating)
                })
                .ToListAsync();

            return Ok(report);
        }

        [HttpGet("actors/most-active")]
        public async Task<ActionResult<ActorWithRolesCountDto>> GetMostActiveActor()
        {
            var report = await _context.Actors
                .Select(a => new ActorWithRolesCountDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    RolesCount = a.Roles.Count
                })
                .OrderByDescending(a => a.RolesCount)
                .FirstOrDefaultAsync();

            return report is null ? NotFound() : Ok(report);
        }

        [HttpGet("movies/with-most-reviews")]
        public async Task<ActionResult<MovieWithReviewsCountDto>> GetFilmsWithMostReviews()
        {
            var report = await _context.Movies
                .Select(m => new MovieWithReviewsCountDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    ReviewsCount = m.Reviews.Count
                })
                .OrderByDescending(m => m.ReviewsCount)
                .FirstOrDefaultAsync();

            return report is null ? NotFound() : Ok(report);
        }

        [HttpGet("genres/popular")]
        public async Task<ActionResult<GenreWithMovieCountDto>> GetMostPopularGenre()
        {
            var report = await _context.Genres
                .Select(g => new GenreWithMovieCountDto
                {
                    Name = g.Name,
                    MoviesCount = g.Movies.Count
                })
                .OrderByDescending(m => m.MoviesCount)
                .FirstOrDefaultAsync();

            return report is null ? NotFound() : Ok(report);
        }
    }
}