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
[Route("api/genres")]
public class GenresController(IServiceManager serviceManager)
    : AppController(serviceManager)
{
    private readonly IGenreService genreService = serviceManager.GenreService;

    /// <summary>
    /// Retrieve genres
    /// </summary>
    /// <returns>List of genres</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GenreDto>>> GetGenres([FromQuery] PageParameters parameters)
    {
        var result = await genreService.GetGenresAsync(parameters);
        this.IncludePaginationMeta(result.Details);
        return Ok(result.Items);
    }
}