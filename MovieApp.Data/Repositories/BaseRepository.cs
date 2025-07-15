using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MovieApp.Core.Contracts;
using MovieApp.Data.Extensions;

namespace MovieApp.Data.Repositories;

public class BaseRepository<T>(MovieContext context)
    : IBaseRepository<T>, IQueryableRepository<T> where T : class, IEntity
{
    protected DbSet<T> DbSet { get; } = context.Set<T>();

    public Task<bool> AnyAsync(Expression<Func<T, bool>> ex) =>
        DbSet.AnyAsync(ex);

    public IQueryable<T> FindBy(Expression<Func<T, bool>>? predicate = null, bool trackChanges = false) =>
        predicate is null
            ? DbSet.WithTracking(trackChanges)
            : DbSet.WithTracking(trackChanges).Where(predicate);

    public IQueryable<T> FindAll(bool trackChanges = false) => // alias
        FindBy(trackChanges: trackChanges);

    public void Attach(T entity) =>
        DbSet.Attach(entity);

    public void Add(T entity) =>
        DbSet.Add(entity);

    public void Update(T entity) =>
        DbSet.Update(entity);

    public void Remove(T entity) =>
        DbSet.Remove(entity);
}