using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Extensions;
using MovieApi.Models.Dtos.Actor;
using MovieApi.Models.Dtos.Movie;
using MovieApi.Models.Entities;

namespace MovieApi.Controllers
{
    [ApiController]
    [Route("api/movies/{movieId}/actors")]
    public class MovieActorsController(MovieContext context, IMapper mapper)
        : AppController(context, mapper)
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActorWithRoleDto>>> GetMovieActors(int movieId)
        {
            if (!await _context.IsPresentAsync<Movie>(movieId))
                return NotFound();

            var actors = await _mapper
                .ProjectTo<ActorWithRoleDto>(
                    _context.QueryById<Movie>(movieId)
                    .SelectMany(m => m.Roles))
                .ToListAsync();

            return Ok(actors);
        }

        [HttpPost]
        public async Task<IActionResult> PostMovieActor(
            [FromRoute] int movieId,
            [FromBody] RoleCreateDto createDto
            )
        {
            var movieMissing = !await _context.IsPresentAsync<Movie>(movieId);
            var actorMissing = !await _context.IsPresentAsync<Actor>(createDto.ActorId);

            if (movieMissing || actorMissing)
                return NotFound();

            var role = _mapper.Map<Role>(createDto);
            role.MovieId = movieId;

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMovieActors), new { movieId }, createDto);
        }
    }
}