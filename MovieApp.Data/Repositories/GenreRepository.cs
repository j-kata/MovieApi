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
        return await query.ToPagedResultAsync(parameters.PageSize, parameters.PageIndex);
    }
}
