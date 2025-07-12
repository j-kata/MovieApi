using MovieApp.Core.Dtos.Actor;
using MovieApp.Core.Dtos.Movie;

namespace MovieApp.Contracts;

public interface IRoleService
{
    public Task<IEnumerable<ActorWithRoleDto>> GetMovieActors(int movieId);
    public Task<RoleCreateDto> PostMovieActor(int movieId, RoleCreateDto createDto);
}