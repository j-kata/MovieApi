using MovieApp.Core.Entities;

namespace MovieApp.Core.Contracts;

public interface IReviewRepository
{
    Task<bool> AnyAsync(int id);
    public void Attach(Review review);
    public void Delete(Review review);
}