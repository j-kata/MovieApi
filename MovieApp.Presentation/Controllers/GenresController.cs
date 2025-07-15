using Microsoft.AspNetCore.Mvc;
using MovieApp.Contracts;
using MovieApp.Core.Dtos.Genre;
using MovieApp.Core.Parameters;
using MovieApp.Presentation.Extensions;

namespace MovieApp.Presentation.Controllers;

/// <summary>
/// Genres controller
/// </summary>
/// <param name="serviceManager">ServiceManager</param>
[ApiController]
[Produces("application/json")]
[Route("api/genres")]
public class GenresController(IServiceManager serviceManager) : ControllerBase
{
    private readonly IGenreService genreService = serviceManager.GenreService;

    /// <summary>
    /// Retrieve genres
    /// </summary>
    /// <param name="parameters">Pagination parameters.</param>
    /// <returns>List of genres</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GenreDto>>> GetGenres(
        [FromQuery] PageParameters parameters)
    {
        var result = await genreService.GetGenresAsync(parameters);
        this.IncludePaginationMeta(result.Details);
        return Ok(result.Items);
    }
}