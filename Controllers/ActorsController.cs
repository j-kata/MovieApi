using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Extensions;
using MovieApi.Models.Dtos.Actor;
using MovieApi.Models.Entities;

namespace MovieApi.Controllers
{
    /// <summary>
    /// Actor controller
    /// </summary>
    /// <param name="context">Context</param>
    /// <param name="mapper">Mapper</param>
    [ApiController]
    [Route("api/actors")]
    public class ActorsController(MovieContext context, IMapper mapper)
        : AppController(context, mapper)
    {
        /// <summary>
        /// Retrieves all actors, optionally filtered by name.
        /// </summary>
        /// <param name="name">Name of the actor</param>
        /// <returns>List of matching actors</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ActorDto>>> GetActors(
            [FromQuery] string? name
        )
        {
            IQueryable<Actor> actors = _context.Actors.AsNoTracking();

            if (name is not null)
                actors = actors.Where(a => EF.Functions.Like(a.Name, $"%{name}%"));

            var result = await _mapper
                .ProjectTo<ActorDto>(actors)
                .ToListAsync();

            return Ok(result);
        }

        /// <summary>
        /// Retrieve actor by id
        /// </summary>
        /// <param name="id">Id of the actor</param>
        /// <returns>Actor with the specified ID, or 404 if not found</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ActorDto>> GetActor(int id)
        {
            var actor = await _mapper
                .ProjectTo<ActorDto>(_context.QueryById<Actor>(id))
                .FirstOrDefaultAsync();

            return actor is null ? NotFound() : Ok(actor);
        }

        /// <summary>
        /// Update actor by id
        /// </summary>
        /// <param name="id">Id of the actor</param>
        /// <param name="updateDto">New values for the actor</param>
        /// <returns>No content if successful, 404 if id not found, 400 if ids do not match</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutActor(int id, ActorUpdateDto updateDto)
        {
            if (id != updateDto.Id)
                return BadRequest();

            var actor = await _context.QueryById<Actor>(id)
                .FirstOrDefaultAsync();

            if (actor is null)
                return NotFound();

            _mapper.Map(updateDto, actor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Create new actor
        /// </summary>
        /// <param name="createDto">Values for the new actor</param>
        /// <returns>New actor if created</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostActor(ActorCreateDto createDto)
        {
            var actor = _mapper.Map<Actor>(createDto);

            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();

            var actorDto = _mapper.Map<ActorDto>(actor);

            return CreatedAtAction("GetActor", new { id = actor.Id }, actorDto);
        }
    }
}