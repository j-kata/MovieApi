using MovieApp.Core.Entities;
using MovieApp.Core.Dtos.Parameters;
using MovieApp.Core.Shared;

namespace MovieApp.Core.Contracts;

public interface IMovieRepository : IBaseRepositoryWithId<Movie>
{
    public Task<PagedResult<Movie>> GetMoviesAsync(
        MovieParameters parameters,
        bool trackChanges = false);

    public Task<Movie?> GetMovieAsync(
        int id,
        bool includeActors = false,
        bool includeDetails = false,
        bool includeReviews = false,
        bool trackChanges = false);
}
