using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MovieApp.Core.Contracts;

namespace MovieApp.Data.Repositories;

public class BaseRepository<T>(MovieContext context)
    : IBaseRepository<T> where T : class, IEntity
{
    protected DbSet<T> DbSet { get; } = context.Set<T>();

    public Task<bool> AnyAsync(Expression<Func<T, bool>> ex) =>
        DbSet.AnyAsync(ex);

    public Task<bool> AnyByIdAsync(int id) =>
        DbSet.AnyAsync(e => EF.Property<int>(e, "Id") == id); // TODO: T with id? interface?

    public void Attach(T entity) =>
        DbSet.Attach(entity);

    public void Add(T entity) =>
        DbSet.Add(entity);

    public void Update(T entity) =>
        DbSet.Update(entity);

    public void Remove(T entity) =>
        DbSet.Remove(entity);
}