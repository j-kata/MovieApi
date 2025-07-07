using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Extensions;
using MovieApi.Models.Dtos.Actor;
using MovieApi.Models.Entities;

namespace MovieApi.Controllers
{
    [ApiController]
    [Route("api/actors")]
    public class ActorsController(MovieContext context, IMapper mapper)
        : AppController(context, mapper)
    {

        [HttpGet]
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

        [HttpGet("{id}")]
        public async Task<ActionResult<ActorDto>> GetActor(int id)
        {
            var actor = await _mapper
                .ProjectTo<ActorDto>(_context.QueryById<Actor>(id))
                .FirstOrDefaultAsync();

            return actor is null ? NotFound() : Ok(actor);
        }

        [HttpPut("{id}")]
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

        [HttpPost]
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