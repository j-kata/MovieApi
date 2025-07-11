using Microsoft.EntityFrameworkCore;
using MovieApp.Core.Contracts;
using MovieApp.Core.Entities;

namespace MovieApp.Data.Repositories;

public class ReviewRepository(MovieContext context)
    : BaseRepository<Review>(context), IReviewRepository
{
    public async Task<IEnumerable<Review>> GetMovieReviewsAsync(int movieId, bool trackChanges = false) =>
        await Find(r => r.MovieId == movieId, trackChanges).ToListAsync();

    public void RemoveById(int id)
    {
        var review = new Review { Id = id };
        Attach(review);
        Remove(review);
    }
}