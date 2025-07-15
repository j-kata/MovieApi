using MovieApp.Core.Entities;
using MovieApp.Core.Dtos.Parameters;
using MovieApp.Core.Shared;

namespace MovieApp.Core.Contracts;

public interface IReviewRepository : IBaseRepositoryWithId<Review>
{
    public Task<PagedResult<Review>> GetMovieReviewsAsync(
        PageParameters parameters,
        int movieId,
        bool newestFirst = true,
        bool trackChanges = false);
}