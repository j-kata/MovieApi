using MovieApp.Core.Contracts;
using MovieApp.Core.Entities;
using MovieApp.Core.Dtos.Parameters;
using MovieApp.Core.Shared;
using MovieApp.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MovieApp.Data.Repositories;

public class ReviewRepository(MovieContext context)
    : BaseRepositoryWithId<Review>(context), IReviewRepository
{
    public async Task<PagedResult<Review>> GetMovieReviewsAsync(
        PageParameters parameters,
        int movieId,
        bool newestFirst = true,
        bool trackChanges = false)
    {
        var query = FindBy(r => r.MovieId == movieId, trackChanges);

        if (newestFirst)
            query = query.OrderByDescending(r => r.CreatedAt);

        return await query.ToPagedResultAsync(parameters.PageSize, parameters.PageIndex);
    }

    public async Task<int> GetMovieReviewsCountAsync(int movieId) =>
        await FindBy(r => r.MovieId == movieId).CountAsync();
}