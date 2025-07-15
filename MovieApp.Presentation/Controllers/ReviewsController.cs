using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MovieApp.Contracts;

namespace MovieApp.Presentation.Controllers;

/// <summary>
/// Reviews controller
/// </summary>
/// <param name="serviceManager">ServiceManager</param>
[ApiController]
[Produces("application/json")]
[Route("api/reviews/{id}")]
public class ReviewsController(IServiceManager serviceManager) : ControllerBase
{
    private readonly IReviewService reviewService = serviceManager.ReviewService;

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
        await reviewService.DeleteReviewAsync(id);
        return NoContent();
    }
}
