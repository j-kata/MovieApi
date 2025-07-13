using Microsoft.EntityFrameworkCore;
using MovieApp.Core.Contracts;
using MovieApp.Data.Extensions;

namespace MovieApp.Data.Repositories;

public class BaseRepositoryWithId<T>(MovieContext context)
    : BaseRepository<T>(context), IBaseRepositoryWithId<T> where T : class, IHasId, new()
{
    public Task<bool> AnyByIdAsync(int id) =>
       DbSet.AnyAsync(e => e.Id == id);

    public Task<T?> GetByIdAsync(int id, bool trackChanges = false) =>
        DbSet.WithTracking(trackChanges)
            .FirstOrDefaultAsync(e => e.Id == id);

    public void RemoveById(int id)
    {
        T stub = new() { Id = id };
        DbSet.Attach(stub);
        DbSet.Remove(stub);
    }
}