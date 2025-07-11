using MovieApp.Core.Entities;

namespace MovieApp.Core.Contracts;

public interface IReviewRepository : IBaseRepository<Review>
{
    public void RemoveById(int id);
    public Task<IEnumerable<Review>> GetMovieReviewsAsync(int movieId, bool trackChanges = false);
}