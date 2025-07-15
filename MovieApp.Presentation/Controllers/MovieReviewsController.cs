using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MovieApp.Core.Dtos.Review;
using MovieApp.Contracts;
using MovieApp.Presentation.Extensions;
using MovieApp.Core.Parameters;

namespace MovieApp.Presentation.Controllers;

/// <summary>
/// Movie reviews controller
/// </summary>
/// <param name="serviceManager">ServiceManager</param>
[ApiController]
[Produces("application/json")]
[Route("api/movies/{movieId}/reviews")]
public class MovieReviewsController(IServiceManager serviceManager) : ControllerBase
{
    private readonly IReviewService reviewService = serviceManager.ReviewService;

    /// <summary>
    /// Retrieve all reviews of a specified movie
    /// </summary>
    /// <param name="movieId">Id of the movie</param>
    /// <param name="parameters">Pagination parameters.</param>
    /// <returns>List of matching reviews, or 404 if movie not found</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetMovieReviews(
        int movieId,
        [FromQuery] PageParameters parameters
    )
    {
        var result = await reviewService.GetReviewsAsync(movieId, parameters);
        this.IncludePaginationMeta(result.Details);
        return Ok(result.Items);
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
        var reviewDto = await reviewService.PostReviewAsync(movieId, createDto);
        return CreatedAtAction("GetMovieReviews", new { movieId }, reviewDto);
    }
}
