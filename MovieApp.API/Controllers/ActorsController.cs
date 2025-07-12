using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Core.Dtos.Actor;
using MovieApp.Core.Entities;
using MovieApp.Core.Contracts;

namespace MovieApp.API.Controllers
{
    /// <summary>
    /// Actors controller
    /// </summary>
    /// <param name="uow">UnitOfWork</param>
    /// <param name="mapper">Mapper</param>
    [Route("api/actors")]
    public class ActorsController(IUnitOfWork uow, IMapper mapper)
        : AppController(uow, mapper)
    {
        /// <summary>
        /// Retrieve all actors, optionally filtered by name.
        /// </summary>
        /// <param name="name">Name of the actor</param>
        /// <returns>List of matching actors</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ActorDto>>> GetActors([FromQuery] string? name)
        {
            var actors = await uow.Actors.GetActorsAsync(name);
            var result = mapper.Map<IEnumerable<ActorDto>>(actors);

            return Ok(result);
        }

        /// <summary>
        /// Retrieve actor by id
        /// </summary>
        /// <param name="id">Id of the actor</param>
        /// <returns>Actor with the specified Id, or 404 if not found</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ActorDto>> GetActor(int id)
        {
            var actor = await uow.Actors.GetByIdAsync(id);
            if (actor == null)
                return NotFound();

            return Ok(mapper.Map<ActorDto>(actor));
        }

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
            if (id != updateDto.Id)
                return BadRequest();

            var actor = await uow.Actors.GetByIdAsync(id, true);

            if (actor is null)
                return NotFound();

            mapper.Map(updateDto, actor);
            await uow.CompleteAsync();

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
            var actor = mapper.Map<Actor>(createDto);

            uow.Actors.Add(actor);
            await uow.CompleteAsync();

            var actorDto = mapper.Map<ActorDto>(actor);

            return CreatedAtAction("GetActor", new { id = actor.Id }, actorDto);
        }
    }
}