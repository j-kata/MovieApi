using Microsoft.EntityFrameworkCore;
using MovieApp.Core.Contracts;
using MovieApp.Core.Entities;

namespace MovieApp.Data.Repositories;

public class ReviewRepository(MovieContext context)
    : BaseRepository<Review>(context), IReviewRepository
{
    public async Task<IEnumerable<Review>> GetMovieReviewsAsync(int movieId, bool trackChanges = false) =>
        await FindBy(r => r.MovieId == movieId, trackChanges).ToListAsync();

    // TODO: rewrite generisk in BaseRepository
    public void RemoveById(int id)
    {
        var review = new Review { Id = id };
        Attach(review);
        Remove(review);
    }
}