using Microsoft.EntityFrameworkCore;
using MovieApp.Core.Contracts;
using MovieApp.Core.Entities;
using MovieApp.Core.Parameters;
using MovieApp.Core.Shared;
using MovieApp.Data.Extensions;

namespace MovieApp.Data.Repositories;

public class ReviewRepository(MovieContext context)
    : BaseRepositoryWithId<Review>(context), IReviewRepository
{
    public async Task<PagedResult<Review>> GetMovieReviewsAsync(int movieId, PageParameters parameters, bool newestFirst = true, bool trackChanges = false)
    {
        var query = FindBy(r => r.MovieId == movieId, trackChanges)
            .WithOffset(parameters.PageSize, parameters.PageIndex);

        if (newestFirst)
            query = query.OrderByDescending(r => r.CreatedAt);

        return new PagedResult<Review>(
            items: await query.ToListAsync(),
            pageIndex: parameters.PageIndex,
            pageSize: parameters.PageSize,
            totalCount: await query.CountAsync()
        );
    }
}