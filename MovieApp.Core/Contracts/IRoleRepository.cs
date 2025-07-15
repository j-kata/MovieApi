using MovieApp.Core.Entities;
using MovieApp.Core.Dtos.Parameters;
using MovieApp.Core.Shared;

namespace MovieApp.Core.Contracts;

public interface IRoleRepository : IBaseRepository<Role>
{
    Task<PagedResult<Role>> GetMovieRolesAsync(PageParameters parameters, int movieId, bool trackChanges = false);
}