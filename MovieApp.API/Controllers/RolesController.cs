using Microsoft.AspNetCore.Mvc;
using MovieApp.Core.Dtos.Actor;
using MovieApp.Core.Dtos.Movie;
using MovieApp.Contracts;

namespace MovieApp.API.Controllers;
/// <summary>
/// Roles controller
/// </summary>
/// <param name="serviceManager">ServiceManager</param>
[Route("api/movies/{movieId}/actors")]
public class RolesController(IServiceManager serviceManager) : AppController(serviceManager)
{
    private readonly IRoleService roleService = serviceManager.RoleService;

    /// <summary>
    /// Retrieve all actors in a specified movie
    /// </summary>
    /// <param name="movieId">Id of the movie</param>
    /// <returns>List of matching actors, or 404 if movie not found</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ActorWithRoleDto>>> GetMovieActors(int movieId) =>
        Ok(await roleService.GetMovieActors(movieId));

    /// <summary>
    /// Add specified actor with role title to movie
    /// </summary>
    /// <param name="movieId">Id of the movie</param>
    /// <param name="createDto">Role information</param>
    /// <returns>Role info if created, 404 if movie or actor not found, 400 if request not valid</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostMovieActor(
        [FromRoute] int movieId,
        [FromBody] RoleCreateDto createDto) =>

        CreatedAtAction(
            nameof(GetMovieActors),
            new { movieId },
            await roleService.PostMovieActor(movieId, createDto));

}
