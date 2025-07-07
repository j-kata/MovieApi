using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieApi.Data;
using MovieApi.Extensions;
using MovieApi.Models.Entities;

namespace MovieApi.Controllers
{
    [ApiController]
    [Route("api/reviews/{id}")]
    public class ReviewsController(MovieContext context, IMapper mapper)
        : AppController(context, mapper)
    {
        [HttpDelete]
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