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
    public class ReviewsController(MovieContext context, IMapper mapper)
        : AppController(context, mapper)
    {
        [HttpGet]
        [Route("api/movies/{movieId}/reviews")]
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
        [Route("api/movies/{movieId}/reviews")]
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

        [HttpDelete]
        [Route("api/reviews/{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            if (!await _context.IsPresentAsync<Review>(id))
                return NotFound();

            var review = _context.AttachStubById<Review>(id);
            _context.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private IQueryable<Review> QueryReviewByMovieId(int movieId) =>
            _context.Reviews.Where(r => r.MovieId == movieId);
    }
}