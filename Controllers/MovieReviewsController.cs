using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Extensions;
using MovieApi.Models.Dtos.Review;
using MovieApi.Models.Entities;

namespace MovieApi.Controllers
{
    [ApiController]
    [Route("api/movies/{movieId}/reviews")]
    public class MovieReviewsController(MovieContext context, IMapper mapper)
        : AppController(context, mapper)
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetMovieReviews(int movieId)
        {
            if (!await _context.IsPresentAsync<Movie>(movieId))
                return NotFound();

            var reviews = await _mapper
                .ProjectTo<ReviewDto>(QueryReviewByMovieId(movieId))
                .ToListAsync();

            return Ok(reviews);
        }

        [HttpPost]
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
            _context.Reviews.Where(r => r.MovieId == movieId);
    }
}