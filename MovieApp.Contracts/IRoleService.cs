using MovieApp.Core.Dtos.Actor;
using MovieApp.Core.Dtos.Role;

namespace MovieApp.Contracts;

public interface IRoleService
{
    public Task<IEnumerable<ActorWithRoleDto>> GetMovieActorsAsync(int movieId);
    public Task PostMovieActorAsync(int movieId, RoleCreateDto createDto);
}