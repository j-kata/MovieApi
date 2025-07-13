using Microsoft.EntityFrameworkCore;
using MovieApp.Core.Contracts;
using MovieApp.Core.Entities;

namespace MovieApp.Data.Repositories;

public class ReviewRepository(MovieContext context)
    : BaseRepositoryWithId<Review>(context), IReviewRepository
{
    public async Task<IEnumerable<Review>> GetMovieReviewsAsync(int movieId, bool newestFirst = true, bool trackChanges = false)
    {
        var query = FindBy(r => r.MovieId == movieId, trackChanges);
        if (newestFirst)
            query = query.OrderByDescending(r => r.CreatedAt);
        return await query.ToListAsync();
    }
}