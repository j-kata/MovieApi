using MovieApp.Core.Entities;

namespace MovieApp.Core.Contracts;

public interface IRoleRepository : IBaseRepository<Role>
{
    Task<IEnumerable<Role>> GetMovieRolesAsync(int movieId, bool trackChanges = false);
}