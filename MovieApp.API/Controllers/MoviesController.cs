// using AutoMapper;
// using Microsoft.AspNetCore.Mvc;
// using MovieApp.Core.Dtos.Movie;
// using MovieApp.Core.Entities;
// using MovieApp.Core.Contracts;
// using MovieApp.Core.Parameters;

// namespace MovieApp.API.Controllers;

// /// <summary>
// /// Movies controller
// /// </summary>
// /// <param name="uow">UnitOfWork</param>
// /// <param name="mapper">Mapper</param>
// [Route("api/movies")]
// public class MoviesController(IUnitOfWork uow, IMapper mapper)
//     : AppController(uow, mapper)
// {
//     /// <summary>
//     /// Retrieve movies, optionally filtered
//     /// </summary>
//     /// <param name="parameters">Optional filter parameters</param>
//     /// <returns>List of matching movies</returns>
//     [HttpGet]
//     [ProducesResponseType(StatusCodes.Status200OK)]
//     public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies(
//         [FromQuery] MovieParameters? parameters
//     )
//     {
//         var movies = await uow.Movies.GetMoviesAsync(parameters);
//         return Ok(mapper.Map<IEnumerable<MovieDto>>(movies));
//     }

//     /// <summary>
//     /// Retrieve movie by id
//     /// </summary>
//     /// <param name="id">Id of the movie</param>
//     /// <param name="withActors">If true, include list of actors</param>
//     /// <returns>Movie with the specified Id, or 404 if not found</returns>
//     [HttpGet("{id}")]
//     [ProducesResponseType(StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     public async Task<IActionResult> GetMovie(
//         int id,
//         [FromQuery] bool withActors = false)
//     {
//         var movie = await uow.Movies.GetMovieAsync(id, includeActors: withActors);
//         if (movie is null)
//             return NotFound();

//         // separate so as not to have actors[] in return when actors are not included
//         return withActors
//             ? Ok(mapper.Map<MovieWithActorsDto>(movie))
//             : Ok(mapper.Map<MovieDto>(movie));
//     }

//     /// <summary>
//     /// Retrieve movie by id with additional details
//     /// </summary>
//     /// <param name="id">Id of the movie</param>
//     /// <returns>Movie with the specified Id, or 404 if not found</returns>
//     [HttpGet("{id}/details")]
//     [ProducesResponseType(StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     public async Task<ActionResult<MovieDetailDto>> GetMovieDetails(int id)
//     {
//         var movie = await uow.Movies.GetMovieAsync(id, includeActors: true, includeReviews: true, includeDetails: true);
//         if (movie is null)
//             return NotFound();

//         return Ok(mapper.Map<MovieDetailDto>(movie));
//     }

//     /// <summary>
//     /// Update movie by id
//     /// </summary>
//     /// <param name="id">Id of the movie</param>
//     /// <param name="updateDto">New values for the movie</param>
//     /// <returns>No content if successful, 404 if movie not found, 400 if request not valid</returns>
//     [HttpPut("{id}")]
//     [ProducesResponseType(StatusCodes.Status204NoContent)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     [ProducesResponseType(StatusCodes.Status400BadRequest)]
//     public async Task<IActionResult> PutMovie(int id, MovieUpdateDto updateDto)
//     {
//         if (!await uow.Genres.AnyByIdAsync(updateDto.GenreId))
//             return BadRequest();

//         if (id != updateDto.Id)
//             return BadRequest();

//         var movie = await uow.Movies.GetMovieAsync(id, includeDetails: true, trackChanges: true);

//         if (movie is null)
//             return NotFound();

//         mapper.Map(updateDto, movie);
//         await uow.CompleteAsync();

//         return NoContent();
//     }

//     /// <summary>
//     /// Create new movie
//     /// </summary>
//     /// <param name="createDto">Values for the new movie</param>
//     /// <returns>New movie if created, 400 if request not valid</returns>
//     [HttpPost]
//     [ProducesResponseType(StatusCodes.Status201Created)]
//     [ProducesResponseType(StatusCodes.Status400BadRequest)]
//     public async Task<ActionResult<MovieDto>> PostMovie(MovieCreateDto createDto)
//     {
//         if (!await uow.Genres.AnyByIdAsync(createDto.GenreId))
//             return BadRequest();

//         var movie = mapper.Map<Movie>(createDto);

//         uow.Movies.Add(movie);
//         await uow.CompleteAsync();

//         // TODO: find better solution for genres
//         //await _context.Entry(movie).Reference(m => m.Genre).LoadAsync(); // load Genre to return Genre.Name in response

//         var movieDto = mapper.Map<MovieDto>(movie);

//         return CreatedAtAction("GetMovie", new { id = movieDto.Id }, movieDto);
//     }

//     /// <summary>
//     /// Delete movie by id
//     /// </summary>
//     /// <param name="id">Id of the movie</param>
//     /// <returns>No content if successful, or 404 if not found</returns>
//     [HttpDelete("{id}")]
//     [ProducesResponseType(StatusCodes.Status204NoContent)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     public async Task<IActionResult> DeleteMovie(int id)
//     {
//         if (!await uow.Movies.AnyByIdAsync(id)) // no need to load the whole object
//             return NotFound();

//         uow.Movies.RemoveById(id);
//         await uow.CompleteAsync();

//         return NoContent();
//     }
// }
