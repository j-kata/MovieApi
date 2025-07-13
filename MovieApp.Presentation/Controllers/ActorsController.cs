using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MovieApp.Core.Dtos.Actor;
using MovieApp.Contracts;

namespace MovieApp.Presentation.Controllers;

/// <summary>
/// Actors controller
/// </summary>
/// <param name="serviceManager">ServiceManager</param>
[Route("api/actors")]
public class ActorsController(IServiceManager serviceManager) : AppController(serviceManager)
{
    private readonly IActorService actorService = serviceManager.ActorService;

    /// <summary>
    /// Retrieve all actors, optionally filtered by name.
    /// </summary>
    /// <param name="name">Name of the actor</param>
    /// <returns>List of matching actors</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ActorDto>>> GetActors([FromQuery] string? name) =>
        Ok(await actorService.GetActorsAsync(name));

    /// <summary>
    /// Retrieve actor by id
    /// </summary>
    /// <param name="id">Id of the actor</param>
    /// <returns>Actor with the specified Id, or 404 if not found</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ActorDto>> GetActor(int id) =>
        Ok(await actorService.GetActorAsync(id));

    /// <summary>
    /// Update actor by id
    /// </summary>
    /// <param name="id">Id of the actor</param>
    /// <param name="updateDto">New values for the actor</param>
    /// <returns>No content if successful, 404 if id not found, 400 if request not valid</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutActor(int id, ActorUpdateDto updateDto)
    {
        await actorService.PutActorAsync(id, updateDto);
        return NoContent();
    }

    /// <summary>
    /// Create new actor
    /// </summary>
    /// <param name="createDto">Values for the new actor</param>
    /// <returns>New actor if created, 400 if request not valid</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostActor(ActorCreateDto createDto)
    {
        var dto = await actorService.PostActorAsync(createDto);
        return CreatedAtAction("GetActor", new { id = dto.Id }, dto);
    }
}
