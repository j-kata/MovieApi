using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Core.Dtos.Review;
using MovieApp.Core.Entities;
using MovieApp.Core.Contracts;

namespace MovieApp.API.Controllers;

/// <summary>
/// Movie reviews controller
/// </summary>
/// <param name="uow">UnitOfWork</param>
/// <param name="mapper">Mapper</param>
[Route("api/movies/{movieId}/reviews")]
public class MovieReviewsController(IUnitOfWork uow, IMapper mapper)
    : AppController(uow, mapper)
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
        if (!await uow.Movies.AnyByIdAsync(movieId))
            return NotFound();

        return Ok(mapper.Map<IEnumerable<ReviewDto>>(
            await uow.Reviews.GetMovieReviewsAsync(movieId)));
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
        // track to return review Id in response
        var movie = await uow.Movies.FindByIdAsync(movieId, trackChanges: true);
        if (movie is null)
            return NotFound();

        var review = mapper.Map<Review>(createDto);
        movie.Reviews.Add(review);

        await uow.CompleteAsync();

        var reviewDto = mapper.Map<ReviewDto>(review);
        return CreatedAtAction("GetMovieReviews", new { movieId }, reviewDto);
    }
}
