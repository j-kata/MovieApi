using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Core.Dtos.Actor;
using MovieApp.Core.Dtos.Movie;
using MovieApp.Core.Entities;
using MovieApp.Core.Contracts;

namespace MovieApp.API.Controllers
{
    /// <summary>
    /// Roles controller
    /// </summary>
    /// <param name="uow">UnitOfWork</param>
    /// <param name="mapper">Mapper</param>
    [Route("api/movies/{movieId}/actors")]
    public class RolesController(IUnitOfWork uow, IMapper mapper)
        : AppController(uow, mapper)
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
            if (!await uow.Movies.AnyByIdAsync(movieId))
                return NotFound();

            var roles = await uow.Roles.GetMovieRolesAsync(movieId);
            var actors = mapper.Map<IEnumerable<ActorWithRoleDto>>(roles);

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
            [FromBody] RoleCreateDto createDto)
        {
            var movieMissing = !await uow.Movies.AnyByIdAsync(movieId);
            var actorMissing = !await uow.Actors.AnyByIdAsync(createDto.ActorId);

            if (movieMissing || actorMissing)
                return NotFound();

            var role = mapper.Map<Role>(createDto);
            role.MovieId = movieId;

            uow.Roles.Add(role);
            await uow.CompleteAsync();

            return CreatedAtAction(nameof(GetMovieActors), new { movieId }, createDto);
        }
    }
}