using Microsoft.EntityFrameworkCore;
using MovieApp.Core.Contracts;
using MovieApp.Core.Entities;

namespace MovieApp.Data.Repositories;

public class RoleRepository(MovieContext context)
    : BaseRepository<Role>(context), IRoleRepository
{
    public async Task<IEnumerable<Role>> GetMovieRolesAsync(int movieId, bool trackChanges = false) =>
        await Find(m => m.MovieId == movieId).Include(r => r.Actor).ToListAsync();
}