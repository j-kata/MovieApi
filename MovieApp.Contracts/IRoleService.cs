using MovieApp.Core.Dtos.Actor;
using MovieApp.Core.Dtos.Role;
using MovieApp.Core.Dtos.Parameters;
using MovieApp.Core.Shared;

namespace MovieApp.Contracts;

public interface IRoleService
{
    public Task<PagedResult<ActorWithRoleDto>> GetMovieActorsAsync(PageParameters pageParameters, int movieId);
    public Task PostMovieActorAsync(int movieId, RoleCreateDto createDto);
}