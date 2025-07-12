using MovieApp.Core.Entities;
using MovieApp.Core.Parameters;

namespace MovieApp.Core.Contracts;

public interface IMovieRepository : IBaseRepository<Movie>
{
    public Task<IEnumerable<Movie>> GetMoviesAsync(MovieParameters? parameters, bool trackChanges = false);
}
