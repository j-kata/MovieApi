using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MovieApp.Core.Dtos.Actor;
using MovieApp.Core.Dtos.Role;
using MovieApp.Contracts;
using MovieApp.Core.Dtos.Parameters;
using MovieApp.Presentation.Extensions;

namespace MovieApp.Presentation.Controllers;
/// <summary>
/// Roles controller
/// </summary>
/// <param name="serviceManager">ServiceManager</param>
[ApiController]
[Produces("application/json")]
[Route("api/movies/{movieId}/actors")]
public class RolesController(IServiceManager serviceManager) : ControllerBase
{
    private readonly IRoleService roleService = serviceManager.RoleService;

    /// <summary>
    /// Retrieve all actors in a specified movie
    /// </summary>
    /// <param name="movieId">Id of the movie</param>
    /// <param name="parameters">Pagination parameters.</param>
    /// <returns>List of matching actors, or 404 if movie not found</returns>
    [HttpGet(Name = "GetMovieActors")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ActorWithRoleDto>>> GetMovieActors(
        [FromQuery] PageParameters parameters, int movieId)
    {
        var result = await roleService.GetMovieActorsAsync(parameters, movieId);
        this.IncludePaginationMeta(result.Details);
        return Ok(result.Items);
    }

    /// <summary>
    /// Add specified actor with role title to movie
    /// </summary>
    /// <param name="movieId">Id of the movie</param>
    /// <param name="createDto">Role information</param>
    /// <returns>Role info if created, 404 if movie or actor not found, 409 if actor is already in a movie</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> PostMovieActor(
        [FromRoute] int movieId,
        [FromBody] RoleCreateDto createDto)
    {
        await roleService.PostMovieActorAsync(movieId, createDto);
        return CreatedAtAction(nameof(GetMovieActors), new { movieId }, null);
    }
}
