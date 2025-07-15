using MovieApp.Core.Entities;
using MovieApp.Core.Dtos.Parameters;
using MovieApp.Core.Shared;

namespace MovieApp.Core.Contracts;

public interface IGenreRepository : IBaseRepositoryWithId<Genre>
{
    public Task<PagedResult<Genre>> GetGenresAsync(PageParameters parameters, bool trackChanges = false);
}