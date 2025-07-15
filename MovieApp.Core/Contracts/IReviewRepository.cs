using MovieApp.Core.Entities;
using MovieApp.Core.Parameters;
using MovieApp.Core.Shared;

namespace MovieApp.Core.Contracts;

public interface IReviewRepository : IBaseRepositoryWithId<Review>
{
    public Task<PagedResult<Review>> GetMovieReviewsAsync(
        int movieId, PageParameters parameters, bool newestFirst = true, bool trackChanges = false);
}