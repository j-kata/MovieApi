using Microsoft.EntityFrameworkCore;
using MovieApp.Core.Contracts;
using MovieApp.Core.Entities;

namespace MovieApp.Data.Repositories;

public class GenreRepository(MovieContext context)
    : BaseRepositoryWithId<Genre>(context), IGenreRepository
{
    public async Task<IEnumerable<Genre>> GetGenresAsync(bool trackChanges = false) =>
        await FindAll(trackChanges: trackChanges).ToListAsync();
}
