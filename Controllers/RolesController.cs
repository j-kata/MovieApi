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
    /// <summary>
    /// Roles controller
    /// </summary>
    /// <param name="context">Context</param>
    /// <param name="mapper">Mapper</param>
    [Route("api/movies/{movieId}/actors")]
    public class RolesController(MovieContext context, IMapper mapper)
        : AppController(context, mapper)
    {
        /// <summary>
        /// Retrieve all actors in a specified movie
        /// </summary>
        /// <param name="movieId">Id of the movie</param>
        /// <returns>List of matching actors, or 404 if movie not found</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Add specified actor with role title to movie
        /// </summary>
        /// <param name="movieId">Id of the movie</param>
        /// <param name="createDto">Role information</param>
        /// <returns>Role info if created, 404 if movie or actor not found, 400 if request not valid</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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