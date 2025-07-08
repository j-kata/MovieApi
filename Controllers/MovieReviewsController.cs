using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Extensions;
using MovieApi.Models.Dtos.Review;
using MovieApi.Models.Entities;

namespace MovieApi.Controllers
{
    /// <summary>
    /// Movie reviews controller
    /// </summary>
    /// <param name="context">Context</param>
    /// <param name="mapper">Mapper</param>
    [ApiController]
    [Route("api/movies/{movieId}/reviews")]
    public class MovieReviewsController(MovieContext context, IMapper mapper)
        : AppController(context, mapper)
    {
        /// <summary>
        /// Retrieve all reviews of a specified movie
        /// </summary>
        /// <param name="movieId">Id of the movie</param>
        /// <returns>List of matching reviews, or 404 if movie not found</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetMovieReviews(int movieId)
        {
            if (!await _context.IsPresentAsync<Movie>(movieId))
                return NotFound();

            var reviews = await _mapper
                .ProjectTo<ReviewDto>(QueryReviewByMovieId(movieId))
                .ToListAsync();

            return Ok(reviews);
        }

        /// <summary>
        /// Create new review of the specified movie
        /// </summary>
        /// <param name="movieId">Id of the movie</param>
        /// <param name="createDto">Review information</param>
        /// <returns>Review if created, 404 if movie not found, 400 if request not valid</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ReviewDto>> PostMovieReview(int movieId, ReviewCreateDto createDto)
        {
            if (!await _context.IsPresentAsync<Movie>(movieId))
                return NotFound();

            var review = _mapper.Map<Review>(createDto);
            review.MovieId = movieId;
            _context.Reviews.Add(review);

            await _context.SaveChangesAsync();

            var reviewDto = _mapper.Map<ReviewDto>(review);

            return CreatedAtAction("GetMovieReviews", new { movieId }, reviewDto);
        }

        private IQueryable<Review> QueryReviewByMovieId(int movieId) =>
            _context.Reviews
            .Where(r => r.MovieId == movieId)
            .OrderByDescending(r => r.CreatedAt);
    }
}