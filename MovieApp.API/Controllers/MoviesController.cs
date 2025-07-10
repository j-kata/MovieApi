using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApp.Data;
using MovieApp.API.Extensions;
using MovieApp.Core.Dtos.Movie;
using MovieApp.Core.Entities;

namespace MovieApp.API.Controllers
{
    /// <summary>
    /// Movies controller
    /// </summary>
    /// <param name="context">Context</param>
    /// <param name="mapper">Mapper</param>
    [Route("api/movies")]
    public class MoviesController(MovieContext context, IMapper mapper)
        : AppController(context, mapper)
    {
        /// <summary>
        /// Retrieve movies, optionally filtered
        /// </summary>
        /// <param name="year">Movie year</param>
        /// <param name="genre">Movie genre</param>
        /// <param name="actor">Actor name</param>
        /// <returns>List of matching movies</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
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

        /// <summary>
        /// Retrieve movie by id
        /// </summary>
        /// <param name="id">Id of the movie</param>
        /// <param name="withActors">If true, include list of actors</param>
        /// <returns>Movie with the specified Id, or 404 if not found</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMovie(
            int id,
            [FromQuery] bool withActors = false)
        {
            if (!await _context.IsPresentAsync<Movie>(id))
                return NotFound();

            var movie = _context.QueryById<Movie>(id);

            // or one dto with optional actors list?
            return withActors
                ? Ok(await _mapper.ProjectTo<MovieWithActorsDto>(movie).FirstOrDefaultAsync())
                : Ok(await _mapper.ProjectTo<MovieDto>(movie).FirstOrDefaultAsync());
        }

        /// <summary>
        /// Retrieve movie by id with additional details
        /// </summary>
        /// <param name="id">Id of the movie</param>
        /// <returns>Movie with the specified Id, or 404 if not found</returns>
        [HttpGet("{id}/details")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MovieDetailDto>> GetMovieDetails(int id)
        {
            var movie = await _mapper
                .ProjectTo<MovieDetailDto>(_context.QueryById<Movie>(id))
                .FirstOrDefaultAsync();

            return movie is null ? NotFound() : Ok(movie);
        }

        /// <summary>
        /// Update movie by id
        /// </summary>
        /// <param name="id">Id of the movie</param>
        /// <param name="updateDto">New values for the movie</param>
        /// <returns>No content if successful, 404 if movie not found, 400 if request not valid</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutMovie(int id, MovieUpdateDto updateDto)
        {
            if (!await _context.IsPresentAsync<Genre>(updateDto.GenreId))
                return BadRequest();

            if (id != updateDto.Id)
                return BadRequest();


            var movie = await _context.Movies
                .Include(m => m.MovieDetail)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie is null)
                return NotFound();

            _mapper.Map(updateDto, movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Create new movie
        /// </summary>
        /// <param name="createDto">Values for the new movie</param>
        /// <returns>New movie if created, 400 if request not valid</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MovieDto>> PostMovie(MovieCreateDto createDto)
        {
            if (!await _context.IsPresentAsync<Genre>(createDto.GenreId))
                return BadRequest();

            var movie = _mapper.Map<Movie>(createDto);

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            await _context.Entry(movie).Reference(m => m.Genre).LoadAsync(); // load Genre to return Genre.Name in response

            var movieDto = _mapper.Map<MovieDto>(movie);

            return CreatedAtAction("GetMovie", new { id = movieDto.Id }, movieDto);
        }

        /// <summary>
        /// Delete movie by id
        /// </summary>
        /// <param name="id">Id of the movie</param>
        /// <returns>No content if successful, or 404 if not found</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            if (!await _context.IsPresentAsync<Movie>(id)) // no need to load the whole object
                return NotFound();

            var movie = _context.AttachStubById<Movie>(id);
            _context.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
