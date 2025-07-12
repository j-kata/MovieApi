using Microsoft.AspNetCore.Mvc;
using MovieApp.Contracts;

namespace MovieApp.API.Controllers;

/// <summary>
/// Reviews controller
/// </summary>
/// <param name="serviceManager">ServiceManager</param>
[Route("api/reviews/{id}")]
public class ReviewsController(IServiceManager serviceManager) : AppController(serviceManager)
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
        await reviewService.DeleteReview(id);
        return NoContent();
    }
}
