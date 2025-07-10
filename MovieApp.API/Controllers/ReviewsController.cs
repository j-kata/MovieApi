using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Data;
using MovieApp.API.Extensions;
using MovieApp.Core.Entities;

namespace MovieApp.API.Controllers
{
    /// <summary>
    /// Reviews controller
    /// </summary>
    /// <param name="context">Context</param>
    /// <param name="mapper">Mapper</param>
    [Route("api/reviews/{id}")]
    public class ReviewsController(MovieContext context, IMapper mapper)
        : AppController(context, mapper)
    {
        /// <summary>
        /// Delete review by id
        /// </summary>
        /// <param name="id">Id of review</param>
        /// <returns>No content if successful, or 404 if not found</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteReview(int id)
        {
            if (!await _context.IsPresentAsync<Review>(id))
                return NotFound();

            var review = _context.AttachStubById<Review>(id);
            _context.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}