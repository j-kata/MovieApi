using MovieApp.Core.Entities;
using MovieApp.Core.Parameters;

namespace MovieApp.Core.Contracts;

public interface IMovieRepository : IBaseRepository<Movie>
{
    public Task<IEnumerable<Movie>> GetMoviesAsync(MovieParameters? parameters, bool trackChanges = false);
    public Task<Movie?> GetMovieAsync(
        int id,
        bool includeActors = false,
        bool includeDetails = false,
        bool includeReviews = false,
        bool trackChanges = false);
    public void RemoveById(int id);
}
