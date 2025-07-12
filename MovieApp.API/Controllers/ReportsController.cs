// using AutoMapper;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using MovieApp.Data;
// using MovieApp.Core.Dtos.Reports;
// using MovieApp.Core.Contracts;

// namespace MovieApp.API.Controllers;

// /// <summary>
// /// Reports controller
// /// </summary>
// /// <param name="uow">UnitOfWork</param>
// /// <param name="mapper">Mapper</param>
// [Route("api/reports")]
// public class ReportsController(IUnitOfWork uow, IMapper mapper)
//     : AppController(uow, mapper)
// {
//     /// <summary>
//     /// Retrieve 5 movies with highest ranking for each genre
//     /// </summary>
//     /// <returns>List of genres with movies</returns>
//     [HttpGet("movies/top5pergenre")]
//     [ProducesResponseType(StatusCodes.Status200OK)]
//     public async Task<ActionResult<IEnumerable<TopMoviesByGenreDto>>> GetTopFiveByGenre()
//     {
//         var report = await _context.Movies
//             .Where(m => m.Reviews.Any())
//             .GroupBy(m => new { m.Genre.Id, m.Genre.Name })
//             .Select(g => new TopMoviesByGenreDto
//             {
//                 Genre = g.Key.Name,
//                 Movies = g.Select(m => new MovieWithRatingDto
//                 {
//                     Id = m.Id,
//                     Title = m.Title,
//                     Rating = Math.Round(m.Reviews.Average(r => r.Rating), 2)
//                 })
//                 .OrderByDescending(m => m.Rating)
//                 .Take(5)
//             })
//             .ToListAsync();

//         return Ok(report);
//     }

//     /// <summary>
//     /// Retrieve average film rating for each genre
//     /// </summary>
//     /// <returns>List of genres with average rating</returns>
//     [HttpGet("movies/average-ratings")]
//     [ProducesResponseType(StatusCodes.Status200OK)]
//     public async Task<ActionResult<IEnumerable<GenreWithRatingDto>>> GetRatingsByGenre()
//     {
//         var report = await _context.Reviews
//             .GroupBy(r => new { r.Movie.Genre.Id, r.Movie.Genre.Name })
//             .Select(g => new GenreWithRatingDto
//             {
//                 Genre = g.Key.Name,
//                 Rating = Math.Round(g.Average(g => g.Rating), 2)
//             })
//             .ToListAsync();

//         return Ok(report);
//     }

//     /// <summary>
//     /// Retrieve actor with highest number of movies
//     /// </summary>
//     /// <returns>Actor, or 404 if not found</returns>
//     [HttpGet("actors/most-active")]
//     [ProducesResponseType(StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     public async Task<ActionResult<ActorWithRolesCountDto>> GetMostActiveActor()
//     {
//         var report = await _context.Actors
//             .Select(a => new ActorWithRolesCountDto
//             {
//                 Id = a.Id,
//                 Name = a.Name,
//                 RolesCount = a.Roles.Count
//             })
//             .OrderByDescending(a => a.RolesCount)
//             .FirstOrDefaultAsync();

//         return report is null ? NotFound() : Ok(report);
//     }

//     /// <summary>
//     /// Retrieve movie with the highest number of reviews
//     /// </summary>
//     /// <returns>Movie, or 404 if not found</returns>
//     [HttpGet("movies/with-most-reviews")]
//     [ProducesResponseType(StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     public async Task<ActionResult<MovieWithReviewsCountDto>> GetFilmsWithMostReviews()
//     {
//         var report = await _context.Movies
//             .Select(m => new MovieWithReviewsCountDto
//             {
//                 Id = m.Id,
//                 Title = m.Title,
//                 ReviewsCount = m.Reviews.Count
//             })
//             .OrderByDescending(m => m.ReviewsCount)
//             .FirstOrDefaultAsync();

//         return report is null ? NotFound() : Ok(report);
//     }

//     /// <summary>
//     /// Retrieve genre with highest film ranking
//     /// </summary>
//     /// <returns>Genre, or 404 if not found</returns>
//     [HttpGet("genres/popular")]
//     public async Task<ActionResult<GenreWithMovieCountDto>> GetMostPopularGenre()
//     {
//         var report = await _context.Genres
//             .Select(g => new GenreWithMovieCountDto
//             {
//                 Name = g.Name,
//                 MoviesCount = g.Movies.Count
//             })
//             .OrderByDescending(m => m.MoviesCount)
//             .FirstOrDefaultAsync();

//         return report is null ? NotFound() : Ok(report);
//     }
// }
