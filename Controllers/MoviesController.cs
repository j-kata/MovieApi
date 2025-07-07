using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Extensions;
using MovieApi.Models.Dtos.Movie;
using MovieApi.Models.Entities;

namespace MovieApi.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesController(MovieContext context, IMapper mapper)
        : AppController(context, mapper)
    {
        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies(
            [FromQuery] int? year,
            [FromQuery] string? genre,
            [FromQuery] string? actor
        )
        {
            IQueryable<Movie> movies = _context.Movies.AsNoTracking();

            if (year is not null)
                movies = movies.Where(m => m.Year == year);
            if (genre is not null)
                movies = movies.Where(m => EF.Functions.Like(m.Genre.Name, genre));
            if (actor is not null)
                movies = movies.Where(m => m.Roles.Any(a => EF.Functions.Like(a.Actor.Name, $"%{actor}%")));

            var result = await _mapper
                .ProjectTo<MovieDto>(movies)
                .ToListAsync();

            return Ok(result);
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovie(int id, [FromQuery] bool withActors = false)
        {
            var movie = QueryMovieById(id);
            if (movie == null)
                return NotFound();

            return withActors
                ? Ok(await _mapper.ProjectTo<MovieWithActorsDto>(movie).FirstOrDefaultAsync())
                : Ok(await _mapper.ProjectTo<MovieDto>(movie).FirstOrDefaultAsync());
        }

        // GET: api/Movies/5/details
        [HttpGet("{id}/details")]
        public async Task<ActionResult<MovieDetailDto>> GetMovieDetails(int id)
        {
            var movie = await _mapper
                .ProjectTo<MovieDetailDto>(QueryMovieById(id))
                .FirstOrDefaultAsync();

            return movie is null ? NotFound() : Ok(movie);
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, MovieUpdateDto updateDto)
        {
            if (id != updateDto.Id)
                return BadRequest();

            var movie = await QueryMovieById(id)
                .Include(m => m.MovieDetail)
                .FirstOrDefaultAsync();

            if (movie is null)
                return NotFound();

            _mapper.Map(updateDto, movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MovieDto>> PostMovie(MovieCreateDto createDto)
        {
            var movie = _mapper.Map<Movie>(createDto);

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            await _context.Entry(movie).Reference(m => m.Genre).LoadAsync(); // load Genre to return Genre.Name in response

            var movieDto = _mapper.Map<MovieDto>(movie);

            return CreatedAtAction("GetMovie", new { id = movieDto.Id }, movieDto);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            if (!await _context.IsPresentAsync<Movie>(id)) // no need to load the whole object
                return NotFound();

            var movie = _context.AttachStubById<Movie>(id);
            _context.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private IQueryable<Movie> QueryMovieById(int id) =>
            _context.Movies.Where(m => m.Id == id);
    }
}
