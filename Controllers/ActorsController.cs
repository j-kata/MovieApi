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
        public async Task<ActionResult<IEnumerable<ActorDto>>> GetActors()
        {
            var actors = await _mapper
                .ProjectTo<ActorDto>(_context.Actors)
                .ToListAsync();

            return Ok(actors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ActorDto>> GetActor(int id)
        {
            var actor = await _mapper
                .ProjectTo<ActorDto>(QueryActorById(id))
                .FirstOrDefaultAsync();

            if (actor == null)
                return NotFound();

            return Ok(actor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutActor(int id, ActorUpdateDto updateDto)
        {
            if (id != updateDto.Id)
                return BadRequest();

            var actor = await QueryActorById(id)
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

        private IQueryable<Actor> QueryActorById(int id) => _context.Actors.Where(a => a.Id == id);
    }
}