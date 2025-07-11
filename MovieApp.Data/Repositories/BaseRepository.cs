using Microsoft.EntityFrameworkCore;
using MovieApp.Core.Contracts;

namespace MovieApp.Data.Repositories;

public class BaseRepository<T>(MovieContext context)
    : IBaseRepository<T> where T : class, IEntity
{
    protected DbSet<T> DbSet { get; } = context.Set<T>();

    public void Attach(T entity) =>
        DbSet.Attach(entity);

    public void Add(T entity) =>
        DbSet.Add(entity);

    public void Update(T entity) =>
        DbSet.Update(entity);

    public void Remove(T entity) =>
        DbSet.Remove(entity);
}