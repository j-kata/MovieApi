using Microsoft.EntityFrameworkCore;
using MovieApp.Core.Contracts;
using MovieApp.Core.Entities;
using MovieApp.Core.Parameters;
using MovieApp.Core.Shared;
using MovieApp.Data.Extensions;

namespace MovieApp.Data.Repositories;

public class GenreRepository(MovieContext context)
    : BaseRepositoryWithId<Genre>(context), IGenreRepository
{
    public async Task<PagedResult<Genre>> GetGenresAsync(PageParameters parameters, bool trackChanges = false)
    {
        var query = FindAll(trackChanges: trackChanges);

        return new PagedResult<Genre>(
            items: await query.WithOffset(parameters.PageSize, parameters.PageIndex).ToListAsync(),
            pageIndex: parameters.PageIndex,
            pageSize: parameters.PageSize,
            totalCount: await query.CountAsync()
        );
    }
}
