using MovieApp.Core.Entities;

namespace MovieApp.Core.Contracts;

public interface IReviewRepository : IBaseRepositoryWithId<Review>
{
    public Task<IEnumerable<Review>> GetMovieReviewsAsync(int movieId, bool newestFirst = true, bool trackChanges = false);
}