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
    [Route("api/movies/{movieId}/actors")]
    public class MovieActorsController(MovieContext context, IMapper mapper)
        : AppController(context, mapper)
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActorDto>>> GetMovieActors(int movieId)
        {
            if (!await _context.IsPresentAsync<Movie>(movieId))
                return NotFound();

            var actors = await _mapper
                .ProjectTo<ActorDto>(_context.Movies.Where(a => a.Id == movieId).SelectMany(a => a.Actors))
                .ToListAsync();

            return Ok(actors);
        }

        [HttpPost]
        [Route("{actorId}")]
        public async Task<IActionResult> PostMovieActor(int movieId, int actorId)
        {
            var movieMissing = !await _context.IsPresentAsync<Movie>(movieId);
            var actorMissing = !await _context.IsPresentAsync<Actor>(actorId);

            if (movieMissing || actorMissing)
                return NotFound();

            var movie = _context.AttachStubById<Movie>(movieId);
            var actor = _context.AttachStubById<Actor>(actorId);

            actor.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}