using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MovieApp.Core.Dtos.Reports;
using MovieApp.Core.Contracts;
using MovieApp.Contracts;

namespace MovieApp.Presentation.Controllers;

/// <summary>
/// Reports controller
/// </summary>
/// <param name="serviceManager">ServiceManager</param>
[Route("api/reports")]
public class ReportsController(IServiceManager serviceManager) : AppController(serviceManager)
{
    private readonly IReportService reportService = serviceManager.ReportService;
    /// <summary>
    /// Retrieve 5 movies with highest ranking for each genre
    /// </summary>
    /// <returns>List of genres with movies</returns>
    [HttpGet("movies/top5pergenre")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TopMoviesByGenreDto>>> GetTopFiveMoviesByGenre() =>
        Ok(await reportService.GetTopMoviesByGenreAsync(5));

    /// <summary>
    /// Retrieve average film rating for each genre
    /// </summary>
    /// <returns>List of genres with average rating</returns>
    [HttpGet("movies/average-ratings")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GenreWithRatingDto>>> GetGenresAvgRating() =>
        Ok(await reportService.GetGenresAvgRatingAsync());

    /// <summary>
    /// Retrieve actor with highest number of movies
    /// </summary>
    /// <returns>Actor, or 404 if not found</returns>
    [HttpGet("actors/most-active")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ActorWithRolesCountDto>> GetMostActiveActor() =>
        Ok(await reportService.GetActorWithMostRolesAsync());

    /// <summary>
    /// Retrieve movie with the highest number of reviews
    /// </summary>
    /// <returns>Movie, or 404 if not found</returns>
    [HttpGet("movies/with-most-reviews")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MovieWithReviewsCountDto>> GetFilmWithMostReviews() =>
        Ok(await reportService.GetFilmWithMostReviewsAsync());

    /// <summary>
    /// Retrieve genre with highest number of films
    /// </summary>
    /// <returns>Genre, or 404 if not found</returns>
    [HttpGet("genres/popular")]
    public async Task<ActionResult<GenreWithMoviesCountDto>> GetMostPopularGenre() =>
        Ok(await reportService.GetGenreWithMostMoviesAsync());
}
