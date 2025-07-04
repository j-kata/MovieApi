using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Models.Dtos.Movie;
using MovieApi.Models.Dtos.Review;
using MovieApi.Models.Entities;

namespace MovieApi.Controllers
{
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly MovieContext _context;
        private readonly IMapper _mapper;

        public ReviewsController(MovieContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [Route("api/movies/{movieId}/reviews")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetMovieReviews(int movieId)
        {
            if (!await IsMoviePresent(movieId))
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
            if (!await IsMoviePresent(movieId))
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
            if (!await IsReviewPresent(id))
                return NotFound();

            var review = new Review { Id = id };
            _context.Entry(review).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private IQueryable<Review> QueryReviewByMovieId(int movieId) =>
            _context.Reviews.Where(r => r.MovieId == movieId);
        private Task<bool> IsMoviePresent(int id) => _context.Movies.AnyAsync(m => m.Id == id);
        private Task<bool> IsReviewPresent(int id) => _context.Reviews.AnyAsync(r => r.Id == id);
    }
}