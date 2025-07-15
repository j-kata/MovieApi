using Microsoft.EntityFrameworkCore;
using MovieApp.Core.Contracts;
using MovieApp.Core.Entities;
using MovieApp.Core.Parameters;
using MovieApp.Core.Shared;
using MovieApp.Data.Extensions;

namespace MovieApp.Data.Repositories;

public class RoleRepository(MovieContext context)
    : BaseRepository<Role>(context), IRoleRepository
{
    public async Task<PagedResult<Role>> GetMovieRolesAsync(PageParameters parameters, int movieId, bool trackChanges = false)
    {
        var query = FindBy(m => m.MovieId == movieId, trackChanges);

        return new PagedResult<Role>(
            items: await query.WithOffset(parameters.PageSize, parameters.PageIndex).Include(r => r.Actor).ToListAsync(),
            pageIndex: parameters.PageIndex,
            pageSize: parameters.PageSize,
            totalCount: await query.CountAsync()
        );
    }
}