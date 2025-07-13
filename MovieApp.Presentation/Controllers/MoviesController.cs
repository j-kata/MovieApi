using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MovieApp.Core.Dtos.Movie;
using MovieApp.Core.Parameters;
using MovieApp.Contracts;

namespace MovieApp.Presentation.Controllers;

/// <summary>
/// Movies controller
/// </summary>
/// <param name="serviceManager">ServiceManager</param>
[Route("api/movies")]
public class MoviesController(IServiceManager serviceManager) : AppController(serviceManager)
{
    private readonly IMovieService movieService = serviceManager.MovieService;

    /// <summary>
    /// Retrieve movies, optionally filtered
    /// </summary>
    /// <param name="parameters">Optional filter parameters</param>
    /// <returns>List of matching movies</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies(
        [FromQuery] MovieParameters? parameters) =>
        Ok(await movieService.GetMoviesAsync(parameters));

    /// <summary>
    /// Retrieve movie by id
    /// </summary>
    /// <param name="id">Id of the movie</param>
    /// <param name="withActors">If true, include list of actors</param>
    /// <returns>Movie with the specified Id, or 404 if not found</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMovie(
        int id,
        [FromQuery] bool withActors = false) =>
        Ok(await movieService.GetMovieAsync(id, withActors));

    /// <summary>
    /// Retrieve movie by id with additional details
    /// </summary>
    /// <param name="id">Id of the movie</param>
    /// <returns>Movie with the specified Id, or 404 if not found</returns>
    [HttpGet("{id}/details")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MovieDetailDto>> GetMovieDetails(int id) =>
        Ok(await movieService.GetMovieDetailsAsync(id));

    /// <summary>
    /// Update movie by id
    /// </summary>
    /// <param name="id">Id of the movie</param>
    /// <param name="updateDto">New values for the movie</param>
    /// <returns>No content if successful, 404 if movie not found, 400 if request not valid</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutMovie(int id, MovieUpdateDto updateDto)
    {
        await movieService.PutMovieAsync(id, updateDto);
        return NoContent();
    }

    /// <summary>
    /// Create new movie
    /// </summary>
    /// <param name="createDto">Values for the new movie</param>
    /// <returns>New movie if created, 400 if request not valid</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MovieDto>> PostMovie(MovieCreateDto createDto)
    {
        var dto = await movieService.PostMovieAsync(createDto);
        return CreatedAtAction("GetMovie", new { id = dto.Id }, dto);
    }

    /// <summary>
    /// Delete movie by id
    /// </summary>
    /// <param name="id">Id of the movie</param>
    /// <returns>No content if successful, or 404 if not found</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        await movieService.DeleteMovieAsync(id);
        return NoContent();
    }
}
