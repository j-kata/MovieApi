using Microsoft.AspNetCore.Mvc;
using MovieApp.Contracts;
using MovieApp.Core.Dtos.Genre;

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
    public async Task<ActionResult<IEnumerable<GenreDto>>> GetGenres() =>
        Ok(await genreService.GetGenresAsync());
}