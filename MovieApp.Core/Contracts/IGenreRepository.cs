using MovieApp.Core.Entities;

namespace MovieApp.Core.Contracts;

public interface IGenreRepository : IBaseRepositoryWithId<Genre>
{
    public Task<IEnumerable<Genre>> GetGenresAsync(bool trackChanges = false);
}