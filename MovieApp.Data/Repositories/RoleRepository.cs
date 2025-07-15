using Microsoft.EntityFrameworkCore;
using MovieApp.Core.Contracts;
using MovieApp.Core.Entities;
using MovieApp.Core.Dtos.Parameters;
using MovieApp.Core.Shared;
using MovieApp.Data.Extensions;

namespace MovieApp.Data.Repositories;

public class RoleRepository(MovieContext context)
    : BaseRepository<Role>(context), IRoleRepository
{
    public async Task<PagedResult<Role>> GetMovieRolesAsync(PageParameters parameters, int movieId, bool trackChanges = false)
    {
        var query = FindBy(m => m.MovieId == movieId, trackChanges).Include(r => r.Actor);
        return await query.ToPagedResultAsync(parameters.PageSize, parameters.PageIndex);
    }
}