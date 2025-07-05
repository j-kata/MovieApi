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
    public class MovieActorsController : ControllerBase
    {
        private readonly MovieContext _context;
        private readonly IMapper _mapper;

        public MovieActorsController(MovieContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActorDto>>> GetMovieActors(int movieId)
        {
            if (!await _context.IsMoviePresentAsync(movieId))
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
            var movieMissing = !await _context.IsMoviePresentAsync(movieId);
            var actorMissing = !await _context.IsActorPresentAsync(actorId);

            if (movieMissing || actorMissing)
                return NotFound();

            var movie = new Movie { Id = movieId };
            var actor = new Actor { Id = actorId };
            _context.AttachRange(movie, actor);

            actor.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}