using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Models;
using MovieApi.Models.Dtos;
using MovieApi.Models.Dtos.Movie;
using MovieApi.Models.Entities;

namespace MovieApi.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieContext _context;
        private readonly IMapper _mapper;

        public MoviesController(MovieContext context, IMapper mapper)
        {
            _context = context; // TODO: validations
            _mapper = mapper;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies()
        {
            var movies = await _mapper
                .ProjectTo<MovieDto>(_context.Movies)
                .ToListAsync();

            return Ok(movies);
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetMovie(int id)
        {
            var movie = await _mapper
                .ProjectTo<MovieDto>(_context.Movies.Where(m => m.Id == id))
                .FirstOrDefaultAsync();

            if (movie == null)
                return NotFound();

            return Ok(movie);
        }

        // GET: api/Movies/5/details
        [HttpGet("{id}/details")]
        public async Task<ActionResult<MovieDetailsDto>> GetMovieDetails(int id)
        {
            var movie = await _mapper
                .ProjectTo<MovieDetailsDto>(_context.Movies.Where(m => m.Id == id))
                .FirstOrDefaultAsync();

            if (movie == null)
                return NotFound();

            return Ok(movie);
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, MovieUpdateDto updateDto)
        {
            if (id != updateDto.Id)
                return BadRequest();

            var movie = await _context.Movies
                .Include(m => m.MovieDetails)
                .Where(m => m.Id == id).FirstOrDefaultAsync();

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

            var movieDto = _mapper.Map<MovieDto>(movie);

            return CreatedAtAction("GetMovie", new { id = movieDto.Id }, movieDto);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.Where(m => m.Id == id).FirstOrDefaultAsync();

            if (movie == null)
                return NotFound();

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
